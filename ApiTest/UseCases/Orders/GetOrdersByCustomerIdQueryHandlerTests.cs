using Application.Common.Interfaces.Repositories.Persistence;
using Application.Dtos;
using Application.UseCases.Orders.Queries;
using AutoMapper;
using Domain.Entities;
using Moq;
using System.Net;

public class GetOrdersByCustomerIdQueryHandlerTests
{
    private readonly Mock<IOrderQueryRepository> _orderQueryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetOrdersByCustomerIdQueryHandler _handler;

    public GetOrdersByCustomerIdQueryHandlerTests()
    {
        _orderQueryRepositoryMock = new Mock<IOrderQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetOrdersByCustomerIdQueryHandler(_orderQueryRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOrders_WhenCustomerIdIsValid()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var orders = new List<Order>
        {
            new Order { Id = Guid.NewGuid(), CustomerId = customerId, OrderStatusId = 1, TotalAmount = 100 }
        };

        var ordersDto = new List<OrderDto>
        {
            new OrderDto { Id = orders[0].Id, CustomerId = customerId, OrderStatusId = 1, TotalAmount = 100 }
        };

        _orderQueryRepositoryMock
           .Setup(repo => repo.GetOrdersByCustomerIdAsync(customerId, It.IsAny<bool>()))
           .ReturnsAsync(orders);

        _mapperMock
            .Setup(mapper => mapper.Map<List<OrderDto>>(orders))
            .Returns(ordersDto);

        var query = new GetOrdersByCustomerIdQuery(customerId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenCustomerIdIsEmpty()
    {
        // Arrange
        var query = new GetOrdersByCustomerIdQuery(Guid.Empty);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal("El Id es requerido", result.Message);
    }
}
