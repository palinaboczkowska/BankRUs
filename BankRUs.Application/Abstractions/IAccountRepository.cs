using BankRUs.Domain.Entities;

namespace BankRUs.Application.Abstractions
{
    public interface IAccountRepository
    {
        Task AddAsync(BankAccount account, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BankAccount>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    }
}
