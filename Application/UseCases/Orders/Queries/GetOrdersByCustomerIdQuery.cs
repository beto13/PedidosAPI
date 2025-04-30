using Application.Dtos;
using Application.Models;
using MediatR;

namespace Application.UseCases.Orders.Queries
{
    public record GetOrdersByCustomerIdQuery(Guid CustomerId) : IRequest<ApiResponse<List<OrderDto>>>;
}
