using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Repositories.Persistence;
using Application.Dtos;
using Application.Filtering.Factories;
using Application.Filtering.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(ISqlQueryRepository<>), typeof(SqlQueryRepository<>));
            services.AddScoped(typeof(ISqlCommandRepository<>), typeof(SqlCommandRepository<>));
            services.AddScoped(typeof(ICustomerQueryRepository), typeof(CustomerQueryRepository));
            services.AddScoped(typeof(IOrderQueryRepository), typeof(OrderQueryRepository));

            services.AddScoped<IFilterService<Customer, CustomerFilterDto>, FilterService<Customer, CustomerFilterDto>>();
            services.AddScoped<IFilterStrategyFactory<Customer, CustomerFilterDto>, CustomerFilterStrategyFactory>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
