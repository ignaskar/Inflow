using Inflow.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Queries;

internal static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(s => s
            .FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services
            .AddSingleton<IQueryDispatcher, QueryDispatcher>();

        return services;
    }
}