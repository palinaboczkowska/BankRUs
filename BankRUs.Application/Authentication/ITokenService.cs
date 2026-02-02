namespace BankRUs.Application.Authentication;

public interface ITokenService
{
    Token CreateToken(string userId, string email, IEnumerable<string>? roles = null);
}
