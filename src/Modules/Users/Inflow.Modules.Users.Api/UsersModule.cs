using Inflow.Modules.Users.Core;
using Inflow.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Modules.Users.Api;

internal class UsersModule : IModule
{
    public string Name { get; } = "Users";
        
    public IEnumerable<string> Policies { get; } = new[]
    {
        "users"
    };

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
    }
        
    public void Use(IApplicationBuilder app)
    {
        
    }
}