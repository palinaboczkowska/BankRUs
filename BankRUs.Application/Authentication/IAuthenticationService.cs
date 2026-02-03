using BankRUs.Application.Authentication.AuthenticateUser;

namespace BankRUs.Application.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticatedUser?> AuthentiateUser(string userName, string password);
}
