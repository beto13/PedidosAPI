using Application.Dtos;
using Application.Models;
using Application.UseCases.Customers.Commands;
using Application.UseCases.Customers.Queries.GetById;
using Application.UseCases.Customers.Queries.GetFiltered;
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
        /// Obtiene los clientes filtrados según los criterios proporcionados.
        /// </summary>
        /// <param name="email">El correo electrónico del cliente. Este parámetro es opcional.</param>
        /// <param name="name">El nombre del cliente. Este parámetro es opcional.</param>
        /// <param name="PageNumber">El número de página para la paginación de los resultados.</param>
        /// <param name="PageSize">La cantidad de resultados por página para la paginación.</param>
        /// <returns>
        /// Devuelve una lista de clientes filtrados, incluyendo detalles como el nombre y correo electrónico.
        /// Si los parámetros de filtrado son incorrectos, devolverá un error con el código 400 y un mensaje descriptivo.
        /// Si no se encuentran clientes que coincidan con los filtros proporcionados, devolverá un error con el código 404.
        /// </returns>
        [HttpGet("get-filter-customers")]
        [SwaggerOperation(Summary = "Obtiene los clientes filtrados", Description = "Permite obtener los detalles de un cliente a partir de diferentes filtros")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<CustomerDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFilterCustomer([FromQuery] string? email, [FromQuery] string? name, [FromQuery] int PageNumber, [FromQuery] int PageSize)
        {
            var filter = new CustomerFilterDto { Email = email, Name = name };

            var response = await mediator.Send(new GetFilteredCustomersQuery(filter, PageNumber, PageSize));
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
