using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Repositories.Persistence;
using Application.Dtos;
using Application.UseCases.Customers.Commands;
using AutoMapper;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest.UseCases.Customers
{
    public class CreateCustomerCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ISqlCommandRepository<Customer>> _customerCmdRepoMock;
        private readonly CreateCustomerCommandHandler _handler;

        public CreateCustomerCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _customerCmdRepoMock = new Mock<ISqlCommandRepository<Customer>>();

            // Setup para que el unitOfWork devuelva el repositorio del cliente
            _unitOfWorkMock.Setup(uow => uow.CommandRepository<Customer>())
                .Returns(_customerCmdRepoMock.Object);

            _handler = new CreateCustomerCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_CustomerCreated_ReturnsSuccessResponse()
        {
            // Arrange
            var customerCreateDto = new CustomerCreateDto
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "123-456-7890"
            };

            var customer = new Customer
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "123-456-7890",
                Orders = new List<Order>   
                {
                    new Order
                    {
                        Id = Guid.NewGuid(),
                        OrderStatusId = 1,  
                        CustomerId = Guid.NewGuid(), 
                        CreatedAt = DateTime.UtcNow
                    }
                }
            };

            var command = new CreateCustomerCommand(customerCreateDto);

            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CustomerDto>())).Returns(customer);
            _mapperMock.Setup(m => m.Map<CustomerCreateDto>(It.IsAny<Customer>())).Returns(customerCreateDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(System.Net.HttpStatusCode.Created, result.StatusCode);
            Assert.Equal("Cliente creado correctamente", result.Message);

            _customerCmdRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}
