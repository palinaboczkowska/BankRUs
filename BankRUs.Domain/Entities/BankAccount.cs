using System.ComponentModel.DataAnnotations;

namespace BankRUs.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }

    [MaxLength(25)]
    public string AccountNumber { get; private set; } = null!;
    public string Name { get; private set; }

    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private BankAccount() { } // för EF

    public BankAccount(string userId, string accountNumber, string name)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AccountNumber = accountNumber;
        Name = name;
        Balance = 0;
        CreatedAt = DateTime.UtcNow;
    }
}