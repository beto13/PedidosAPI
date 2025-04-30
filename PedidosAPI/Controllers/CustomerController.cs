using Application.Dtos;
using Application.Models;
using Application.UseCases.Customers.Commands;
using Application.UseCases.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace PedidosAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator mediator;

        public CustomerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Obtiene un cliente por su ID.
        /// </summary>
        /// <param name="id">El ID del cliente.</param>
        /// <returns>Devuelve el cliente con la información solicitada.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtiene un cliente por su ID", Description = "Permite obtener los detalles de un cliente a partir de su ID.")]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var response = await mediator.Send(new GetCustomerByIdQuery(id));
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="dto">El DTO del cliente que contiene los datos para crear un nuevo cliente.</param>
        /// <returns>Devuelve una respuesta con el cliente creado.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Crea un nuevo cliente", Description = "Permite crear un nuevo cliente con la información proporcionada.")]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CustomerCreateDto dto)
        {
            var response = await mediator.Send(new CreateCustomerCommand(dto));
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
