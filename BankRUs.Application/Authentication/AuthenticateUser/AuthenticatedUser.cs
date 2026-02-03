namespace BankRUs.Application.Authentication.AuthenticateUser;

public record AuthenticatedUser(string UserId, string UserName, string Email, IEnumerable<string> Roles = null);