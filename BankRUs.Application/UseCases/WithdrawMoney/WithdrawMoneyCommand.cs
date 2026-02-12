namespace BankRUs.Application.UseCases.WithdrawMoney;

public sealed record WithdrawMoneyCommand(
    string UserId,
    Guid BankAccountId,
    decimal Amount,
    string? Reference
);