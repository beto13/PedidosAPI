using Application.Dtos;
using Application.Models;
using MediatR;

namespace Application.UseCases.Customers.Queries
{
    public record GetCustomerByIdQuery(Guid Id) : IRequest<ApiResponse<CustomerDto>>;
}
