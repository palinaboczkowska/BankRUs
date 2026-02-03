using BankRUs.Api.Dtos.Auth;
using BankRUs.Application.Authentication.AuthenticateUser;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthenticateUserHandler _authenticateUserHandler;

    public AuthController(AuthenticateUserHandler authenticateUserHandler)
    {
        _authenticateUserHandler = authenticateUserHandler;
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequest)
    {
        var authenticateUserResult = await _authenticateUserHandler.HandleAsync(
          new AuthenticateUserCommand(
            loginRequest.UserName,
            loginRequest.Password));

        if (!authenticateUserResult.Succeed)
        {
            return Unauthorized(); // 401 Unauthorized
        }

        var reponse = new LoginResponseDto(
            Token: authenticateUserResult.AccessToken,
            ExpiredAtUtc: authenticateUserResult.ExpiresAtUtc);

        return Ok(reponse);
    }
}
