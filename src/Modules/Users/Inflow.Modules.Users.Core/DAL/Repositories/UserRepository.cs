using Inflow.Modules.Users.Core.Entities;
using Inflow.Modules.Users.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Users.Core.DAL.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;

    public UserRepository(UsersDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetAsync(Guid id)
        => _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);

    public Task<User?> GetAsync(string email)
        => _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}