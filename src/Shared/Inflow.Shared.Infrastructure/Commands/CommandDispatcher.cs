using Inflow.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Commands;

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CommandDispatcher(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken ct = default) where TCommand : class, ICommand
    {
        if (command is null)
        {
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, ct);
    }
}