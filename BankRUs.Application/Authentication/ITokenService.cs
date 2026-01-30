namespace BankRUs.Application.Authentication;

public interface ITokenService
{
    Token CreateToken(string UserId, string Email);
}
