namespace BankRUs.Api.Dtos.Transactions;

public class GetTransactionsQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Type { get; set; } // deposit | withdrawal
    public string Sort { get; set; } = "desc"; // asc | desc
}