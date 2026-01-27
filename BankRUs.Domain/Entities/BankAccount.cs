using System.ComponentModel.DataAnnotations;

namespace BankRUs.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    [MaxLength(25)]
    public string AccountNumber { get; private set; } = null!;
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private BankAccount() { } // för EF

    public BankAccount(Guid userId, string accountNumber)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AccountNumber = accountNumber;
        Balance = 0;
        CreatedAt = DateTime.UtcNow;
    }
}