using Application.Dtos;
using Application.Models;
using MediatR;

namespace Application.UseCases.Customers.Commands
{
    public record CreateCustomerCommand(CustomerCreateDto Dto) : IRequest<ApiResponse<CustomerDto>>;
}
