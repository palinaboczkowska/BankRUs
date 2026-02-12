namespace BankRUs.Api.Dtos.Transactions
{
    public sealed record DepositRequest(
    decimal Amount,
    string? Reference
);

}
