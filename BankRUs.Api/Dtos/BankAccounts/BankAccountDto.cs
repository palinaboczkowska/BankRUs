namespace BankRUs.Api.Dtos.BankAccounts
{
    public record BankAccountDto
        (
        Guid AccountId, 
        string AccountNumber, 
        decimal Balance
        );
}
