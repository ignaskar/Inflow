using Inflow.Modules.Users.Core.Entities;
using Inflow.Modules.Users.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Users.Core.DAL.Repositories;

internal class RoleRepository : IRoleRepository
{
    private readonly UsersDbContext _context;

    public RoleRepository(UsersDbContext context)
    {
        _context = context;
    }

    public Task<Role?> GetAsync(string name) => _context.Roles.FirstOrDefaultAsync(x => x.Name == name);

    public async Task<IReadOnlyList<Role>> GetAllAsync() => await _context.Roles.ToListAsync();

    public async Task AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
    }
}