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

    public async Task<IReadOnlyList<BankAccount>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.BankAccounts
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<BankAccount?> GetByIdAsync(Guid bankAccountId)
    {
        return await _dbContext.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == bankAccountId);
    }



    public async Task UpdateAsync(BankAccount account)
    {
        _dbContext.BankAccounts.Update(account);
        await _dbContext.SaveChangesAsync();
    }
}