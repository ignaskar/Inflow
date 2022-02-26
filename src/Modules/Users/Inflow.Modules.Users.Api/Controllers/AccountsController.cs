using Inflow.Modules.Users.Core.Commands;
using Inflow.Modules.Users.Core.DTO;
using Inflow.Modules.Users.Core.Queries;
using Inflow.Modules.Users.Core.Services;
using Inflow.Shared.Abstractions.Dispatchers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inflow.Modules.Users.Api.Controllers;

[ApiController]
[Route("[controller]")]
internal class AccountsController : ControllerBase
{
    private const string AccessTokenCookie = "__access-token";
    private readonly IDispatcher _dispatcher;
    private readonly IUserRequestStorage _userRequestStorage;
    private readonly CookieOptions _cookieOptions;

    public AccountsController(
        IDispatcher dispatcher, 
        IUserRequestStorage userRequestStorage,
        CookieOptions cookieOptions)
    {
        _dispatcher = dispatcher;
        _userRequestStorage = userRequestStorage;
        _cookieOptions = cookieOptions;
    }

    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp(SignUp command, CancellationToken ct)
    {
        await _dispatcher.SendAsync(command, ct);
        return NoContent();
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<UserDetailsDto>> SignIn(SignIn command, CancellationToken ct)
    {
        await _dispatcher.SendAsync(command, ct);
        var jwt = _userRequestStorage.GetToken(command.Id);
        var user = await _dispatcher.QueryAsync(new GetUser {UserId = jwt.UserId}, ct);
        AddCookie(AccessTokenCookie, jwt.AccessToken);
        return Ok(user);
    }

    private void AddCookie(string key, string value)
        => Response.Cookies.Append(key, value, _cookieOptions);
}