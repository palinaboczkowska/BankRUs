using BankRUs.Application.Abstractions;
using BankRUs.Domain;

namespace BankRUs.Application.UseCases.DepositMoney;

public sealed class DepositMoneyHandler
{
    private readonly IBankAccountRepository _accounts;
    private readonly ITransactionRepository _transactions;

    public DepositMoneyHandler(
        IBankAccountRepository accounts,
        ITransactionRepository transactions)
    {
        _accounts = accounts;
        _transactions = transactions;
    }

    public async Task<DepositResult> Handle(DepositMoneyCommand command)
    {
        if (command.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        if (decimal.Round(command.Amount, 2) != command.Amount)
            throw new ArgumentException("Amount must have max 2 decimals");

        if (command.Reference?.Length > 140)
            throw new ArgumentException("Reference too long");

        var account = await _accounts.GetByIdAsync(command.BankAccountId);

        if (account is null || account.UserId != command.UserId)
            throw new KeyNotFoundException("Account not found");

        account.Balance += command.Amount;
        await _accounts.UpdateAsync(account);

        var transaction = await _transactions.CreateDepositAsync(
            account.Id,
            command.UserId,
            command.Amount,
            command.Reference,
            account.Balance
        );

        return new DepositResult(
            TransactionId: transaction.Id,
            UserId: command.UserId,
            Type: "deposit",
            Amount: command.Amount,
            Currency: "SEK",
            Reference: command.Reference,
            CreatedAt: transaction.CreatedAt,
            BalanceAfter: account.Balance
        );
    }
}