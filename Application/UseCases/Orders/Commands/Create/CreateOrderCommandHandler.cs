using Application.Common.Interfaces.Repositories;
using Application.Dtos;
using Application.Models;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.UseCases.Orders.Commands.Create
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResponse = await ValidateRequestAsync(request.Dto);
            if (validationResponse != null) return validationResponse;

            var order = MapToOrderEntity(request.Dto);

            await unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await SaveOrderAsync(order);

                await UpdateProductStockAsync(request.Dto.OrderItems);
            });

            return new ApiResponse<bool>(HttpStatusCode.Created, "Orden creada exitosamente", true, true);
        }

        private async Task<ApiResponse<bool>?> ValidateRequestAsync(OrderDto dto)
        {
            if (dto.CustomerId == Guid.Empty)
                return new ApiResponse<bool>(HttpStatusCode.BadRequest, "El ID del cliente es requerido.", false);

            if (dto.OrderItems == null || !dto.OrderItems.Any())
                return new ApiResponse<bool>(HttpStatusCode.BadRequest, "La orden debe contener al menos un producto.", false);

            var customerQryRepository = unitOfWork.QueryRepository<Customer>();
            var customer = await customerQryRepository.GetByIdAsync(dto.CustomerId);

            if (customer == null)
                return new ApiResponse<bool>(HttpStatusCode.BadRequest, $"El cliente con ID {dto.CustomerId} no existe.", false);

            var productQryRepository = unitOfWork.QueryRepository<Product>();
            foreach (var item in dto.OrderItems)
            {
                var product = await productQryRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    return new ApiResponse<bool>(HttpStatusCode.BadRequest, $"El producto con ID {item.ProductId} no existe.", false);

                if (item.Quantity > product.StockQuantity)
                    return new ApiResponse<bool>(HttpStatusCode.BadRequest, $"No hay suficiente stock para el producto {product.Name}. Stock disponible: {product.StockQuantity}.", false);
            }

            return null;
        }

        private static Order MapToOrderEntity(OrderDto dto)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                CreatedAt = DateTime.UtcNow,
                OrderStatusId = dto.OrderStatusId,
                OrderItems = dto.OrderItems.Select(item => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
        }

        private async Task SaveOrderAsync(Order order)
        {
            var orderCmdRepository = unitOfWork.CommandRepository<Order>();
            await orderCmdRepository.AddAsync(order);
        }

        private async Task UpdateProductStockAsync(IEnumerable<OrderItemDto> orderItems)
        {
            var productQryRepository = unitOfWork.QueryRepository<Product>();
            var productCmdRepository = unitOfWork.CommandRepository<Product>();

            foreach (var item in orderItems)
            {
                var product = await productQryRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                    productCmdRepository.UpdateProperties(product, p => p.StockQuantity);
                }
            }
        }
    }
}
