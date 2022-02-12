using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Customers.Core.Exceptions;

internal class CustomerAlreadyCompletedException : InflowException
{
    public Guid CustomerId { get; }

    public CustomerAlreadyCompletedException(Guid customerId) : base($"Customer with ID: '{customerId}' is already completed")
    {
        CustomerId = customerId;
    }
}