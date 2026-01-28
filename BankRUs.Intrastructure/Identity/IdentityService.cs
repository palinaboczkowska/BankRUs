
using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Intrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResult> CreateUserAsync(CreateUserRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email.Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            SocialSecurityNumber = request.SocialSecurityNumber.Trim(),
            Email = request.Email.Trim()
        };

        string password = "Secret#1";

        // TODO: Skapa användaren i databasen (ASP.NET Core Identity)
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new CreateUserResult(null, errors);
        }

        await _userManager.AddToRoleAsync(user, Roles.Customer);

        return new CreateUserResult(Guid.Parse(user.Id), []);

    }
}
