using BankRUs.Api.Dtos.BankAccounts;

namespace BankRUs.Api.Dtos.Users
{
    public record CustomerDetailsDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    IReadOnlyList<BankAccountResponseDto> BankAccounts
);
}
