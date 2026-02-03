namespace BankRUs.Application.Identity;

public record CreateUserResult(string? UserId, List<string> Errors)
{
    public bool Succeeded => Errors == null || Errors.Count == 0;
}
