using System.Runtime.CompilerServices;
using Inflow.Modules.Customers.Core.DAL;
using Inflow.Modules.Customers.Core.DAL.Repositories;
using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Inflow.Modules.Customers.Api")]
namespace Inflow.Modules.Customers.Core;

internal static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddPostgres<CustomersDbContext>(configuration);

        return services;
    }
}