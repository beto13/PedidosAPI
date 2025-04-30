using Application.Common.Interfaces.Repositories;
using Application.Models;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.UseCases.Orders.Commands.Update
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateOrderStatusHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orderQryRepository = unitOfWork.QueryRepository<Order>();
            var orderCmdRepository = unitOfWork.CommandRepository<Order>();

            var order = await orderQryRepository.GetByIdAsync(request.OrderId);

            if (order == null)
                return new ApiResponse<bool>(HttpStatusCode.NotFound, $"No se encontró la orden con ID {request.OrderId}", false);

            if (!IsValidStatusTransition(order.OrderStatusId, request.StatusId))
                return new ApiResponse<bool>(HttpStatusCode.BadRequest, "Transición de estado inválida", false);

            order.OrderStatusId = request.StatusId;
            order.UpdatedAt = DateTime.UtcNow;

            orderCmdRepository.UpdateProperties(order, o => o.OrderStatusId, o => o.UpdatedAt!);

            await unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(HttpStatusCode.OK, "Estado actualizado correctamente", true);
        }

        private bool IsValidStatusTransition(int currentStatus, int newStatus)
        {
            var validStatuses = new List<int> { 1, 2, 3, 4, 5 }; 

            if (!validStatuses.Contains(currentStatus) || !validStatuses.Contains(newStatus))
                return false;

            return newStatus == currentStatus + 1 || (currentStatus == 5 && newStatus == 4);
        }
    }
}
