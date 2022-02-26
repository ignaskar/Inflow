using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Inflow.Shared.Infrastructure.Postgres;

internal sealed class DbContextAppInitializer : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DbContextAppInitializer> _logger;

    public DbContextAppInitializer(IServiceScopeFactory scopeFactory, ILogger<DbContextAppInitializer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var dbContextTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsInterface && x != typeof(DbContext));

        using var scope = _scopeFactory.CreateScope();
        foreach (var dbContextType in dbContextTypes)
        {
            var dbContext = scope.ServiceProvider.GetService(dbContextType) as DbContext;
            if (dbContext is null)
            {
                continue;
            }

            _logger.LogInformation($"Running DbContext for module {dbContextType.Name}");
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        var initializers = scope.ServiceProvider.GetServices<IInitializer>();
        foreach (var initializer in initializers)
        {
            try
            {
                _logger.LogInformation($"Running the initializer: {initializer.GetType().Name}...");
                await initializer.InitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}