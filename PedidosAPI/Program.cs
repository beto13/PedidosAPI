using Application.Configuration;
using Application.Validators;
using FluentValidation;
using Infrastructure.Configuration;
using Microsoft.OpenApi.Models;
using PedidosAPI.Middlewares;

namespace PedidosAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pedidos API",
                    Version = "v1",
                    Description = "API para gestionar pedidos"
                });
            });

            builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<OrderItemValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<OrderValidator>();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pedidos API v1");
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
