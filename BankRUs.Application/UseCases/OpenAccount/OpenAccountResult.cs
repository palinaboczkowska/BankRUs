namespace BankRUs.Application.UseCases.OpenAccount;

public record OpenAccountResult(Guid UserId, Guid AccountId, string AccountNumber);
