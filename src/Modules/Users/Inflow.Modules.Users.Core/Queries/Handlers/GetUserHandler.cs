using Inflow.Modules.Users.Core.DAL;
using Inflow.Modules.Users.Core.DTO;
using Inflow.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Users.Core.Queries.Handlers;

internal sealed class GetUserHandler : IQueryHandler<GetUser, UserDetailsDto>
{
    private readonly UsersDbContext _context;

    public GetUserHandler(UsersDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserDetailsDto> HandleAsync(GetUser query, CancellationToken ct = default)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == query.UserId, ct);

        return user?.AsDetailsDto();
    }
}