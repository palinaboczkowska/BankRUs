using BankRUs.Domain.Entities;

namespace BankRUs.Application.Abstractions
{
    public interface IBankAccountRepository
    {
        Task AddAsync(BankAccount account, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BankAccount>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<BankAccount?> GetByIdAsync(Guid id);
        Task UpdateAsync(BankAccount account);

    }
}
