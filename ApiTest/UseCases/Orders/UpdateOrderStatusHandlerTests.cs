using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Repositories.Persistence;
using Application.UseCases.Orders.Commands.Update;
using Domain.Entities;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace ApiTest.UseCases.Orders
{
    public class UpdateOrderStatusHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISqlCommandRepository<Order>> _orderCmdRepoMock;
        private readonly Mock<ISqlQueryRepository<Order>> _orderQryRepoMock;
        private readonly UpdateOrderStatusHandler _handler;

        public UpdateOrderStatusHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _orderCmdRepoMock = new Mock<ISqlCommandRepository<Order>>();
            _orderQryRepoMock = new Mock<ISqlQueryRepository<Order>>();

            _unitOfWorkMock.Setup(x => x.CommandRepository<Order>()).Returns(_orderCmdRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.QueryRepository<Order>()).Returns(_orderQryRepoMock.Object);

            _handler = new UpdateOrderStatusHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidTransition_ReturnsSuccess()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, OrderStatusId = 1 };

            _orderQryRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            var command = new UpdateOrderStatusCommand(orderId, 2);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Estado actualizado correctamente", result.Message);

            _orderCmdRepoMock.Verify(r => r.UpdateProperties(order, It.IsAny<Expression<Func<Order, object>>[]>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidTransition_ReturnsBadRequest()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, OrderStatusId = 1 };

            _orderQryRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            var command = new UpdateOrderStatusCommand(orderId, 5);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Transición de estado inválida", result.Message);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsNotFound()
        {
            var orderId = Guid.NewGuid();

            _orderQryRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Order?)null);

            var command = new UpdateOrderStatusCommand(orderId, 2);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("No se encontró la orden", result.Message);
        }
    }
}
