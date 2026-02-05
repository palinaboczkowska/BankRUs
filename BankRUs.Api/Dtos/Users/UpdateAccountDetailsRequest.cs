namespace BankRUs.Api.Dtos.Users
{
    public sealed record UpdateAccountDetailsRequest(
    string? FirstName,
    string? LastName,
    string? Email,
    string? SocialSecurityNumber
);

}
