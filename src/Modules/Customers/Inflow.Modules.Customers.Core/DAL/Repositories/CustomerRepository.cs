using Inflow.Modules.Customers.Core.Domain.Entities;
using Inflow.Modules.Customers.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Customers.Core.DAL.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    private readonly CustomersDbContext _context;

    public CustomerRepository(CustomersDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string name)
        => await _context.Customers.AnyAsync(x => x.Name == name);

    public async Task<Customer?> GetAsync(Guid id)
        => await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Customer? customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }
}