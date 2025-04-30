using Application.Dtos;
using Application.Models;
using Application.UseCases.Orders.Commands.Create;
using Application.UseCases.Orders.Commands.Update;
using Application.UseCases.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace PedidosAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Crea un nuevo pedido", Description = "Permite crear un nuevo pedido para un cliente.")]
        [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(OrderDto dto)
        {
            var response = await mediator.Send(new CreateOrderCommand(dto));
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{customerId}/orders")]
        [SwaggerOperation(Summary = "Obtiene los pedidos de un cliente", Description = "Permite obtener todos los pedidos realizados por un cliente.")]
        [ProducesResponseType(typeof(ApiResponse<List<OrderDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<OrderDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<OrderDto>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrdersByCustomerId(Guid customerId)
        {
            var response = await mediator.Send(new GetOrdersByCustomerIdQuery(customerId));
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{id}/status/{statusId}")]
        [SwaggerOperation(Summary = "Actualiza el estado de un pedido", Description = "Permite actualizar el estado de un pedido específico usando el ID del pedido y el ID del nuevo estado.")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(Guid id, int statusId)
        {
            var result = await mediator.Send(new UpdateOrderStatusCommand(id, statusId));
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
