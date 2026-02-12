using BankRUs.Domain.Entities;

namespace BankRUs.Application.Abstractions;

public interface ITransactionRepository
{
    Task<Transaction> CreateDepositAsync(
        Guid bankAccountId,
        string userId,
        decimal amount,
        string? reference,
        decimal balanceAfter,
        CancellationToken cancellationToken = default);
}
