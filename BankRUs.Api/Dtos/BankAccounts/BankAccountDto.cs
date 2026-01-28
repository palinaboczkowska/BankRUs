namespace BankRUs.Api.Dtos.BankAccounts
{
    public record BankAccountDto
        (
        string Name,
        Guid AccountId, 
        string AccountNumber,
        decimal Balance
        );
}
