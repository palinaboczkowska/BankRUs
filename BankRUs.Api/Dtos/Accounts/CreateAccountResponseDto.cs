namespace BankRUs.Api.Dtos.Accounts;

public record CreateAccountResponseDto(string UserId, Guid AccountId, string AccountNumber);
