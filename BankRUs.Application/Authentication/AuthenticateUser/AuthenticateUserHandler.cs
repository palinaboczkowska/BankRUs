namespace BankRUs.Application.Authentication.AuthenticateUser;

public sealed class AuthenticateUserHandler
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;

    public AuthenticateUserHandler(
        IAuthenticationService authenticationService,
        ITokenService tokenService)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
    }

    public async Task<AuthenticateUserResult> HandleAsync(AuthenticateUserCommand command)
    {
        // 1. Kontrollera om det finns en användare med angivet 
        //    användarnamn och lösenord
        var authenticatedUser = await _authenticationService.AuthentiateUser(
            userName: command.UserName,
            password: command.Password);

        if (authenticatedUser == null)
        {
            return AuthenticateUserResult.Failed();
        }

        var token = _tokenService.CreateToken(
            userId: authenticatedUser.UserId,
            email: authenticatedUser.Email,
            roles: authenticatedUser.Roles);

        // 3. Returnera resultat
        return AuthenticateUserResult.Succeeded(
          accessToken: token.AccessToken,
          expiresAtUtc: token.ExpiresAtUtc
        );
    }
}
