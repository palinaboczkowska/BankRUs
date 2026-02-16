using BankRUs.Application.Abstractions;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.GetTransactions;

public class GetTransactionsHandler
{
    private readonly IBankAccountRepository _accounts;
    private readonly ITransactionRepository _transactions;

    public GetTransactionsHandler(
        IBankAccountRepository accounts,
        ITransactionRepository transactions)
    {
        _accounts = accounts;
        _transactions = transactions;
    }

    public async Task<(BankAccount account, List<Transaction> items, int totalCount)> Handle(GetTransactionsCommand cmd)
    {
        var account = await _accounts.GetByIdAsync(cmd.AccountId);
        if (account is null || account.UserId != cmd.UserId)
            throw new KeyNotFoundException("Account not found");

        var (items, totalCount) = await _transactions.GetForAccountAsync(
            cmd.AccountId,
            cmd.From,
            cmd.To,
            cmd.Type,
            cmd.Sort,
            cmd.Page,
            cmd.PageSize
        );
        return (account, items, totalCount);
    }
}