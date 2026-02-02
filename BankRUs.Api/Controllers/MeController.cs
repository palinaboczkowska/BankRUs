using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankRUs.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Customer")]
[ApiController]
public class MeController : ControllerBase
{
    // GET /api/me
    [HttpGet]
    public IActionResult Get()
    {
        // User = HttpContext.User
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // GetAccountDetailsCommand(userId)

        var email = User.FindFirstValue(ClaimTypes.Email);
        var userName = User.Identity?.Name ?? User.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var response = new MeResponseDto(
          UserId: userId,
          UserName: userName ?? "",
          Email: email ?? ""
        );

        return Ok(response);

    }
}
