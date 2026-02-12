namespace BankRUs.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BankAccountId { get; set; }
    public BankAccount Account { get; set; } = default!;

    public string UserId { get; set; }

    public TransactionType Type { get; set; }

    public decimal Amount { get; set; }
    public string Currency { get; set; } = "SEK";
    public string? Reference { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public decimal BalanceAfter { get; set; }

    public Transaction() { }

    public Transaction(
        Guid bankAccountId,
        string userId,
        TransactionType type,
        decimal amount,
        string? reference,
        decimal balanceAfter)
    {
        Id = Guid.NewGuid();
        BankAccountId = bankAccountId;
        UserId = userId;
        Type = type;
        Amount = amount;
        Reference = reference;
        BalanceAfter = balanceAfter;
        CreatedAt = DateTime.UtcNow;
    }
}
