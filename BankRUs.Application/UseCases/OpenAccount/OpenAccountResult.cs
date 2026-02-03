namespace BankRUs.Application.UseCases.OpenAccount;

public record OpenAccountResult(string UserId, Guid AccountId, string AccountNumber);
