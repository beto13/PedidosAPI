using Application.Dtos;
using Application.Filtering.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.UseCases.Customers.Queries.GetFiltered
{
    internal class GetFilteredCustomersQueryHandler : IRequestHandler<GetFilteredCustomersQuery, ApiResponse<PagedResult<CustomerDto>>>
    {
        private readonly IFilterService<Customer, CustomerFilterDto> filterService;
        private readonly IMapper mapper;

        public GetFilteredCustomersQueryHandler(IFilterService<Customer, CustomerFilterDto> filterService, IMapper mapper)
        {
            this.filterService = filterService;
            this.mapper = mapper;
        }
        public async Task<ApiResponse<PagedResult<CustomerDto>>> Handle(GetFilteredCustomersQuery request, CancellationToken cancellationToken)
        {

            var parameters = new PaginationParameters { PageNumber = request.PageNumber, PageSize = request.PageSize };

            var result = await filterService.Execute(request.FilterDto, parameters);

            var dtoList = mapper.Map<List<CustomerDto>>(result.Data);

            var pagedDto = new PagedResult<CustomerDto>
            {
                Data = dtoList,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };

            return new ApiResponse<PagedResult<CustomerDto>>(HttpStatusCode.OK, "", true, pagedDto);
        }
    }
}
