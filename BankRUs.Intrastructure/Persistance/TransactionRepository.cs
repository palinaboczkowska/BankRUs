using BankRUs.Application.Abstractions;
using BankRUs.Domain;
using BankRUs.Domain.Entities;
using BankRUs.Domain;
using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Persistence;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _db;

    public TransactionRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Transaction> CreateDepositAsync(
        Guid bankAccountId,
        string userId,
        decimal amount,
        string? reference,
        decimal balanceAfter,
        CancellationToken cancellationToken = default)
    {
        var transaction = new Transaction(
            bankAccountId,
            userId,
            TransactionType.Deposit,
            amount,
            reference,
            balanceAfter
        );

        await _db.Transactions.AddAsync(transaction, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return transaction;
    }
}