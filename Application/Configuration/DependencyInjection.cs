using Application.Common.Mapping;
using Application.UseCases.Customers.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;

namespace Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<bool>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));
    
            services.AddAutoMapper(typeof(AssemblyReference).Assembly);
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}
