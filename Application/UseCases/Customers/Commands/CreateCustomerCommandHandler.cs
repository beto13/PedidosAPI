using Application.Common.Interfaces.Repositories;
using Application.Dtos;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Customers.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ApiResponse<CustomerDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerCmdRepository = unitOfWork.CommandRepository<Customer>();

            var customer = mapper.Map<Customer>(request.Dto);

            await customerCmdRepository.AddAsync(customer);

            await unitOfWork.SaveChangesAsync();

            var customerDto = mapper.Map<CustomerDto>(customer);

            return new ApiResponse<CustomerDto>(System.Net.HttpStatusCode.Created, "Cliente creado correctamente", true, customerDto) ;
        }
    }
}
