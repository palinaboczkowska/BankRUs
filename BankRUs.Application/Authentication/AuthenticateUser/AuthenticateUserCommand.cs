namespace BankRUs.Application.Authentication.AuthenticateUser;

public sealed record AuthenticateUserCommand(
  string UserName,
  string Password
);
