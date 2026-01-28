using BankRUs.Application.Abstractions;
using BankRUs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Persistance;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BankAccountRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(BankAccount account, CancellationToken cancellationToken = default)
    {
        await _dbContext.BankAccounts.AddAsync(account, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BankAccount>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.BankAccounts
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}