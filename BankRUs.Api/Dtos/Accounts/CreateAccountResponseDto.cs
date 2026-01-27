namespace BankRUs.Api.Dtos.Accounts;

public record CreateAccountResponseDto(Guid UserId, Guid AccountId, string AccountNumber);
