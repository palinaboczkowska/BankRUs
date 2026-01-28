namespace BankRUs.Application.Identity;

public record CreateUserResult(Guid? UserId, List<string> Errors)
{
    public bool Succeeded => Errors == null || Errors.Count == 0;
}
