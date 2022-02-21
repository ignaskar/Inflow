using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Exceptions;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class VerifyCustomerHandler : ICommandHandler<VerifyCustomer>
{
    private readonly ICustomerRepository _repository;
    private readonly IClock _clock;
    private readonly ILogger<VerifyCustomerHandler> _logger;

    public VerifyCustomerHandler(ICustomerRepository repository, IClock clock, ILogger<VerifyCustomerHandler> logger)
    {
        _repository = repository;
        _clock = clock;
        _logger = logger;
    }
    
    public async Task HandleAsync(VerifyCustomer command, CancellationToken ct = default)
    {
        var customer = await _repository.GetAsync(command.CustomerId);
        if (customer is null) throw new CustomerNotFoundException(command.CustomerId);
        customer.Verify(_clock.CurrentDateTime());
        await _repository.UpdateAsync(customer);
        _logger.LogInformation($"Verified a customer with ID: '{command.CustomerId}'");
    }
}