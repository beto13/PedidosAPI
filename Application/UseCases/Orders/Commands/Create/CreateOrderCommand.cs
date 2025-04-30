using Application.Dtos;
using Application.Models;
using MediatR;

namespace Application.UseCases.Orders.Commands.Create
{
    public record CreateOrderCommand(OrderDto Dto) : IRequest<ApiResponse<bool>>;
}
