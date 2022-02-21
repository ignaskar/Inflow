using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Exceptions;
using Inflow.Shared.Abstractions.Commands;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class LockCustomerHandler : ICommandHandler<LockCustomer>
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<LockCustomerHandler> _logger;

    public LockCustomerHandler(ICustomerRepository repository, ILogger<LockCustomerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task HandleAsync(LockCustomer command, CancellationToken ct = default)
    {
        var customer = await _repository.GetAsync(command.CustomerId);
        if (customer is null) throw new CustomerNotFoundException(command.CustomerId);
        customer.Lock(command.Notes);
        await _repository.UpdateAsync(customer);
        _logger.LogInformation($"Locked a customer with ID: '{command.CustomerId}'");
    }
}