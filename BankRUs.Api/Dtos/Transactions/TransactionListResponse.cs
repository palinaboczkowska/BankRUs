namespace BankRUs.Api.Dtos.Transactions;

public class TransactionListResponse
{
    public Guid AccountId { get; set; }
    public string Currency { get; set; } = "SEK";
    public decimal Balance { get; set; }
    public PagingInfo Paging { get; set; } = default!;
    public List<TransactionDto> Items { get; set; } = new();
}

public class PagingInfo
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}