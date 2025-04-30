using Application.Common.Interfaces.Repositories.Persistence;
using Application.Dtos;
using Application.Models;
using AutoMapper;
using MediatR;
using System.Net;

namespace Application.UseCases.Customers.Queries
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerDto>>
    {
        private readonly ICustomerQueryRepository customerQueryRepository;
        private readonly IMapper mapper;

        public GetCustomerByIdQueryHandler(ICustomerQueryRepository customerQueryRepository, IMapper mapper)
        {
            this.customerQueryRepository = customerQueryRepository;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return new ApiResponse<CustomerDto>(HttpStatusCode.BadRequest, "El Id es requerido", false);

            var customer = await customerQueryRepository.GetCustomerWithOrdersByIdAsync(request.Id);

            if (customer == null)
                return new ApiResponse<CustomerDto>(HttpStatusCode.NotFound, "Cliente no encontrado", false, null!);

            var customerDto = mapper.Map<CustomerDto>(customer);

            return new ApiResponse<CustomerDto>(System.Net.HttpStatusCode.OK, true, customerDto);
        }
    }
}
