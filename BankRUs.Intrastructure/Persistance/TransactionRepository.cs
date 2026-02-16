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

    public async Task<(List<Transaction> items, int totalCount)> GetForAccountAsync(
    Guid accountId,
    DateTime? from,
    DateTime? to,
    string? type,
    string sort,
    int page,
    int pageSize)
    {
        var query = _db.Transactions
            .Where(t => t.BankAccountId == accountId)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(t => t.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.CreatedAt <= to.Value);

        if (!string.IsNullOrEmpty(type))
        {
            if (type == "deposit")
                query = query.Where(t => t.Type == TransactionType.Deposit);
            else if (type == "withdrawal")
                query = query.Where(t => t.Type == TransactionType.Withdrawal);
        }

        query = sort == "asc"
            ? query.OrderBy(t => t.CreatedAt)
            : query.OrderByDescending(t => t.CreatedAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}