using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class CustomerNotFoundException : InflowException
{
    public Guid CustomerId { get; }
    
    public CustomerNotFoundException(Guid customerId) : base($"Customer with ID: '{customerId}' not found.")
    {
        CustomerId = customerId;
    }
}