using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Repositories.Persistence;
using Application.Dtos;
using Application.UseCases.Orders.Commands.Create;
using Domain.Entities;
using Moq;
using System.Net;

namespace ApiTest.UseCases.Orders
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateOrderCommandHandler(_unitOfWorkMock.Object);
        }


        [Fact]
        public async Task Handle_ValidOrder_ShouldReturnSuccess()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var customerRepoMock = new Mock<ISqlQueryRepository<Customer>>();
            customerRepoMock.Setup(repo => repo.GetByIdAsync(customerId))
                            .ReturnsAsync(new Customer { Id = customerId });

            var productRepoMock = new Mock<ISqlQueryRepository<Product>>();
            productRepoMock.Setup(repo => repo.GetByIdAsync(productId))
                           .ReturnsAsync(new Product { Id = productId, StockQuantity = 10, Name = "TestProduct" });

            var orderCmdRepoMock = new Mock<ISqlCommandRepository<Order>>();
            var productCmdRepoMock = new Mock<ISqlCommandRepository<Product>>();

            _unitOfWorkMock.Setup(u => u.QueryRepository<Customer>()).Returns(customerRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.QueryRepository<Product>()).Returns(productRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.CommandRepository<Order>()).Returns(orderCmdRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.CommandRepository<Product>()).Returns(productCmdRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.ExecuteTransactionAsync(It.IsAny<Func<Task>>()))
                           .Returns<Func<Task>>(func => func());

            var orderDto = new OrderDto
            {
                CustomerId = customerId,
                OrderStatusId = 1,
                OrderItems = new List<OrderItemDto>
            {
                new OrderItemDto { ProductId = productId, Quantity = 2, UnitPrice = 100 }
            }
            };

            var command = new CreateOrderCommand(orderDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }
    }
}
