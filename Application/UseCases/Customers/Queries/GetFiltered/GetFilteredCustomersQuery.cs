using Application.Dtos;
using Application.Models;
using MediatR;

namespace Application.UseCases.Customers.Queries.GetFiltered
{
    public record GetFilteredCustomersQuery(CustomerFilterDto FilterDto, int PageNumber, int PageSize) : IRequest<ApiResponse<PagedResult<CustomerDto>>>;
}
