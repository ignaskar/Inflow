using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Queries;

namespace Inflow.Shared.Abstractions.Dispatchers;

public interface IDispatcher
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken ct = default)
        where TCommand : class, ICommand;
    
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct = default)
        where TResult : class;
}