using Inflow.Modules.Customers.Core.Commands;
using Inflow.Modules.Customers.Core.DTO;
using Inflow.Modules.Customers.Core.Queries;
using Inflow.Shared.Abstractions.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace Inflow.Modules.Customers.Api.Controllers;

[ApiController]
[Route("[controller]")]
internal class CustomersController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public CustomersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<CustomerDetailsDto>> GetCustomerById(Guid customerId)
    {
        var customer = await _dispatcher.QueryAsync(new GetCustomer {CustomerId = customerId});
        if (customer is null) 
            return NotFound();
        
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomer(CreateCustomer command, CancellationToken ct)
    {
        await _dispatcher.SendAsync(command, ct);
        return NoContent();
    }
}