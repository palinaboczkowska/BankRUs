namespace BankRUs.Api.Dtos.Transactions
{
    public record TransactionDto(
    Guid TransactionId,
    decimal Amount,
    string? Reference,
    DateTime CreatedAt,
    string Type
);

}
