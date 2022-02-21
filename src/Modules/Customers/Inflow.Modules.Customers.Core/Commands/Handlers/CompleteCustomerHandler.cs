using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Domain.ValueObjects;
using Inflow.Modules.Customers.Core.Exceptions;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class CompleteCustomerHandler : ICommandHandler<CompleteCustomer>
{
    private readonly ICustomerRepository _repository;
    private readonly IClock _clock;
    private readonly ILogger<CompleteCustomerHandler> _logger;

    public CompleteCustomerHandler(ICustomerRepository repository, IClock clock, ILogger<CompleteCustomerHandler> logger)
    {
        _repository = repository;
        _clock = clock;
        _logger = logger;
    }
    
    public async Task HandleAsync(CompleteCustomer command, CancellationToken ct = default)
    {
        var customer = await _repository.GetAsync(command.CustomerId);
        if (customer is null) 
            throw new CustomerNotFoundException(command.CustomerId);
        
        if (!string.IsNullOrWhiteSpace(command.Name) && await _repository.ExistsAsync(command.Name))
            throw new CustomerAlreadyExistsException(command.Name);
        
        customer.Complete(
            command.Name, 
            command.FullName, 
            command.Address,
            command.Nationality,
            new Identity(command.IdentityType, command.IdentitySeries),
            _clock.CurrentDateTime());

        await _repository.UpdateAsync(customer);
        _logger.LogInformation($"Completed a customer with ID: '{command.CustomerId}'");
    }
}