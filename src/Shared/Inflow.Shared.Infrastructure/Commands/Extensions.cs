using Inflow.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Commands;

internal static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(s => s
            .FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services
            .AddSingleton<ICommandDispatcher, CommandDispatcher>();

        return services;
    }
}