using System.Reflection;
using System.Runtime.CompilerServices;
using Inflow.Shared.Abstractions.Dispatchers;
using Inflow.Shared.Abstractions.Storage;
using Inflow.Shared.Abstractions.Time;
using Inflow.Shared.Infrastructure.Api;
using Inflow.Shared.Infrastructure.Auth;
using Inflow.Shared.Infrastructure.Commands;
using Inflow.Shared.Infrastructure.Dispatchers;
using Inflow.Shared.Infrastructure.Postgres;
using Inflow.Shared.Infrastructure.Queries;
using Inflow.Shared.Infrastructure.Storage;
using Inflow.Shared.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInitializer<T>(this IServiceCollection services) where T : class, IInitializer
        => services.AddTransient<IInitializer, T>();
    
    public static IServiceCollection AddModularInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration,
        IList<Assembly> assemblies)
    {
        var disabledModules = new List<string>();
        foreach (var (key, value) in configuration.AsEnumerable())
        {
            if (!key.Contains(":module:enabled"))
            {
                continue;
            }

            if (!bool.Parse(value))
            {
                disabledModules.Add(key.Split(':')[0]);
            }
        }
        
        services
            .AddMemoryCache()
            .AddAuth(configuration)
            .AddSingleton<IRequestStorage, RequestStorage>()
            .AddCommands(assemblies)
            .AddQueries(assemblies)
            .AddSingleton<IDispatcher, InMemoryDispatcher>()
            .AddPostgres(configuration)
            .AddSingleton<IClock, UtcClock>()
            .AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                var removedParts = new List<ApplicationPart>();
                foreach (var disabledModule in disabledModules)
                {
                    var parts = manager.ApplicationParts
                        .Where(x => x.Name.Contains(disabledModule, StringComparison.InvariantCultureIgnoreCase));
                    removedParts.AddRange(parts);
                }

                foreach (var removedPart in removedParts)
                {
                    manager.ApplicationParts.Remove(removedPart);
                }
                
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });

        return services;
    }

    public static IApplicationBuilder UseModularInfrastructure(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        app.UseAuth();
        app.UseAuthorization();
        
        return app;
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName)
        where T : class, new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }
}