using Application.Models;
using FluentValidation;
using PedidosAPI.Exceptions;
using System.Net;
using System.Text.Json;

namespace PedidosAPI.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            // Manejo de errores de validación
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => e.ErrorMessage);

                var response = new ApiResponse<object>(
                    statusCode: HttpStatusCode.BadRequest,
                    message: ex.Message,
                    success: false);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            // Manejo de error de recurso no encontrado
            catch (KeyNotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";

                var response = new ApiResponse<object>(
                                    statusCode: HttpStatusCode.NotFound,
                                    message: ex.Message,
                                    success: false);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            // Manejo de errores de autorización
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var response = new ApiResponse<object>(
                                    statusCode: HttpStatusCode.Unauthorized,
                                    message: ex.Message,
                                    success: false);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            // Manejo de errores de acceso prohibido
            catch (ForbiddenAccessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.ContentType = "application/json";

                var response = new ApiResponse<object>(
                                    statusCode: HttpStatusCode.Forbidden,
                                    message: ex.Message,
                                    success: false);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            // Manejo de errores inesperados
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new ApiResponse<object>(
                                    statusCode: HttpStatusCode.InternalServerError,
                                    message: ex.Message,
                                    success: false);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
