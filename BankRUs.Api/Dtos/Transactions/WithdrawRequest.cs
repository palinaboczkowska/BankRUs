namespace BankRUs.Api.Dtos.Transactions;

public class WithdrawRequest
{
    public decimal Amount { get; set; }
    public string? Reference { get; set; }
}