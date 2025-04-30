using Application.Models;
using MediatR;

namespace Application.UseCases.Orders.Commands.Update
{
    public record UpdateOrderStatusCommand(Guid OrderId, int StatusId) : IRequest<ApiResponse<bool>>;

}
