using Inflow.Modules.Customers.Core.Domain.Entities;
using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;
using Inflow.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class CreateCustomerHandler : ICommandHandler<CreateCustomer>
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly IClock _clock;

    public CreateCustomerHandler(ICustomerRepository repository, ILogger<CreateCustomerHandler> logger, IClock clock)
    {
        _repository = repository;
        _logger = logger;
        _clock = clock;
    }

    public async Task HandleAsync(CreateCustomer command, CancellationToken ct = default)
    {
        // Simply acts as a guard in case we'll make some calls to other services
        _ = new Email(command.Email);

        var customer = new Customer(command.Email, _clock.CurrentDateTime());
        await _repository.AddAsync(customer);
        _logger.LogInformation($"Created customer with ID: '{customer.Id}'");
    }
}