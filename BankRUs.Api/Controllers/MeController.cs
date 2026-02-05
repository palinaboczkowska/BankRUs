using BankRUs.Api.Dtos.Users;
using BankRUs.Application.Abstractions;
using BankRUs.Application.UseCases.UpdateAccountDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankRUs.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Customer")]
[ApiController]
public class MeController : ControllerBase
{
    private readonly IUserRepository _repo;

    public MeController(IUserRepository repo)
    {
        _repo = repo;
    }

    // GET /api/me
    [HttpGet]
    public IActionResult Get()
    {
        // User = HttpContext.User
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // GetAccountDetailsCommand(userId)

        var email = User.FindFirstValue(ClaimTypes.Email);
        var userName = email;


        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var response = new MeResponseDto(
          UserId: userId,
          UserName: userName ?? "",
          Email: email ?? ""
        );

        return Ok(response);

    }

    // PATCH /api/me
    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateAccountDetailsRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var command = new UpdateAccountDetailsCommand(
            UserId: userId,
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            SocialSecurityNumber: request.SocialSecurityNumber
        );

        var handler = new UpdateAccountDetailsHandler(_repo);
        await handler.Handle(command);

        return NoContent();
    }


}
