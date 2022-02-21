using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Exceptions;
using Inflow.Shared.Abstractions.Commands;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class UnlockCustomerHandler : ICommandHandler<UnlockCustomer>
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<UnlockCustomerHandler> _logger;

    public UnlockCustomerHandler(ICustomerRepository repository, ILogger<UnlockCustomerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task HandleAsync(UnlockCustomer command, CancellationToken ct = default)
    {
        var customer = await _repository.GetAsync(command.CustomerId);
        if (customer is null) throw new CustomerNotFoundException(command.CustomerId);
        customer.Unlock(command.Notes);
        await _repository.UpdateAsync(customer);
        _logger.LogInformation($"Unlocked a customer with ID: '{command.CustomerId}'");
    }
}