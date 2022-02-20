using Inflow.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Queries;

internal class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceScopeFactory _scopeFactory;

    public QueryDispatcher(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct = default) where TResult : class
    {
        using var scope = _scopeFactory.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));
        if (method is null)
        {
            throw new InvalidOperationException("Query handler is invalid");
        }

        // need to cast this to a task because the invocation will return an object which cannot be awaited.
        return await (Task<TResult>) method.Invoke(handler, new object[] { query, ct })!;
    }
}