namespace Inflow.Shared.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken ct = default)
        where TCommand : class, ICommand;
}