using Inflow.Modules.Customers.Core.Commands;
using Inflow.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Inflow.Modules.Customers.Api.Controllers;

[ApiController]
[Route("[controller]")]
internal class CustomersController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;

    public CustomersController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomer(CreateCustomer command, CancellationToken ct)
    {
        await _commandDispatcher.SendAsync(command, ct);
        return NoContent();
    }
}