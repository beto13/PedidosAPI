using Application.Common.Interfaces.Repositories.Persistence;
using Application.Dtos;
using Application.UseCases.Customers.Queries;
using AutoMapper;
using Domain.Entities;
using Moq;
using System.Net;

namespace ApiTest.UseCases.Customers
{
    public class GetCustomerByIdQueryHandlerTests
    {
        private readonly Mock<ICustomerQueryRepository> _customerQueryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCustomerByIdQueryHandler _handler;

        public GetCustomerByIdQueryHandlerTests()
        {
            // Crear mocks
            _customerQueryRepositoryMock = new Mock<ICustomerQueryRepository>();
            _mapperMock = new Mock<IMapper>();

            // Instanciar el handler con los mocks
            _handler = new GetCustomerByIdQueryHandler(_customerQueryRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_CustomerExists_ReturnsOk()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                Name = "John Doe",
                Email = "johndoe@example.com"
            };

            var customerDto = new CustomerDto
            {
                Id = customerId,
                Name = "John Doe",
                Email = "johndoe@example.com"
            };

            // Setup mocks
            _customerQueryRepositoryMock
                .Setup(repo => repo.GetCustomerWithOrdersByIdAsync(customerId, It.IsAny<bool>()))
                .ReturnsAsync(customer);

            _mapperMock
                .Setup(m => m.Map<CustomerDto>(customer))
                .Returns(customerDto);

            var query = new GetCustomerByIdQuery(customerId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(customerDto, result.Data);
        }

        [Fact]
        public async Task Handle_CustomerNotFound_ReturnsNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            // Setup mocks
            _customerQueryRepositoryMock
                .Setup(repo => repo.GetCustomerWithOrdersByIdAsync(customerId, It.IsAny<bool>()))
                .ReturnsAsync((Customer)null); 

            var query = new GetCustomerByIdQuery(customerId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Cliente no encontrado", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var query = new GetCustomerByIdQuery(Guid.Empty); 

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("El Id es requerido", result.Message);
        }
    }
}
