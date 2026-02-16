using BankRUs.Domain;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.Abstractions;

public interface ITransactionRepository
{
    Task<Transaction> CreateAsync(
    Guid bankAccountId,
    string userId,
    TransactionType type,
    decimal amount,
    string? reference,
    decimal balanceAfter,
    CancellationToken cancellationToken = default);

    Task<(List<Transaction> items, int totalCount)> GetForAccountAsync(
    Guid accountId,
    DateTime? from,
    DateTime? to,
    string? type,
    string sort,
    int page,
    int pageSize);
}
