using BankRUs.Application.Authentication;
using BankRUs.Application.Authentication.AuthenticateUser;
using BankRUs.Intrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Intrastructure.Autentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthenticatedUser?> AuthentiateUser(string userName, string password)
    {
        // Kontroller att användarnamn + lösenord är korrekt
        var user = await _userManager.FindByEmailAsync(userName);

        if (user == null) return null;

        var validPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!validPassword) return null;

        return new AuthenticatedUser(
            UserId: user.Id,
            UserName: user.UserName!,
            Email: user.Email!
        );
    }
}
