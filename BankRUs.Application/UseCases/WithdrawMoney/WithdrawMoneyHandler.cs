using BankRUs.Application.Abstractions;
using BankRUs.Domain;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.WithdrawMoney;

public class WithdrawMoneyHandler
{
    private readonly IBankAccountRepository _accounts;
    private readonly ITransactionRepository _transactions;

    public WithdrawMoneyHandler(
        IBankAccountRepository accounts,
        ITransactionRepository transactions)
    {
        _accounts = accounts;
        _transactions = transactions;
    }

    public async Task<Transaction> Handle(WithdrawMoneyCommand command)
    {
        if (command.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        var account = await _accounts.GetByIdAsync(command.BankAccountId);

        if (account is null || account.UserId != command.UserId)
            throw new KeyNotFoundException("Account not found");

        if (account.Balance < command.Amount)
            throw new InvalidOperationException(
                $"Insufficient funds: balance is {account.Balance} but withdrawal is {command.Amount}"
            );

        account.Balance -= command.Amount;
        await _accounts.UpdateAsync(account);

        var transaction = await _transactions.CreateAsync(
            account.Id,
            command.UserId,
            TransactionType.Withdrawal,
            command.Amount,
            command.Reference,
            account.Balance
        );

        return transaction;
    }
}