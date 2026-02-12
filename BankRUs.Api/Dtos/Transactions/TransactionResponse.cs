namespace BankRUs.Api.Dtos.Transactions;

public class TransactionResponse
{
    public Guid TransactionId { get; set; }
    public Guid AccountId { get; set; }
    public string Type { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "SEK";
    public string? Reference { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal BalanceAfter { get; set; }
}