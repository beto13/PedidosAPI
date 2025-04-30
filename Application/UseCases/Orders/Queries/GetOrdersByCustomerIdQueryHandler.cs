using Application.Common.Interfaces.Repositories.Persistence;
using Application.Dtos;
using Application.Models;
using AutoMapper;
using MediatR;
using System.Net;

namespace Application.UseCases.Orders.Queries
{
    public class GetOrdersByCustomerIdQueryHandler : IRequestHandler<GetOrdersByCustomerIdQuery, ApiResponse<List<OrderDto>>>
    {
        private readonly IOrderQueryRepository orderQueryRepository;
        private readonly IMapper mapper;

        public GetOrdersByCustomerIdQueryHandler(IOrderQueryRepository orderQueryRepository, IMapper mapper)
        {
            this.orderQueryRepository = orderQueryRepository;
            this.mapper = mapper;
        }
        public async Task<ApiResponse<List<OrderDto>>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            if (request.CustomerId == Guid.Empty)
                return new ApiResponse<List<OrderDto>> (HttpStatusCode.BadRequest, "El Id es requerido", false);

            var orders = await orderQueryRepository.GetOrdersByCustomerIdAsync(request.CustomerId);

            var ordersDto = mapper.Map<List<OrderDto>>(orders);

            return new ApiResponse<List<OrderDto>>(HttpStatusCode.OK, true, ordersDto);
        }
    }
}
