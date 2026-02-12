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

    public async Task<Transaction> CreateAsync(
        Guid bankAccountId,
        string userId,
        TransactionType type,
        decimal amount,
        string? reference,
        decimal balanceAfter,
        CancellationToken cancellationToken = default)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            BankAccountId = bankAccountId,
            UserId = userId,
            Type = type,
            Amount = amount,
            Reference = reference,
            CreatedAt = DateTime.UtcNow,
            BalanceAfter = balanceAfter
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync(cancellationToken);

        return transaction;
    }

}