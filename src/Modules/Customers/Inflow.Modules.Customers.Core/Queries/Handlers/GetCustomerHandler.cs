using Inflow.Modules.Customers.Core.DAL;
using Inflow.Modules.Customers.Core.DTO;
using Inflow.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Customers.Core.Queries.Handlers;

internal sealed class GetCustomerHandler : IQueryHandler<GetCustomer, CustomerDetailsDto>
{
    private readonly CustomersDbContext _dbContext;

    public GetCustomerHandler(CustomersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<CustomerDetailsDto> HandleAsync(GetCustomer query, CancellationToken ct = default)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.CustomerId, ct);

        return customer?.AsDetailsDto();
    }
}