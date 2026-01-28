namespace BankRUs.Api.Dtos.BankAccounts
{
    public record BankAccountResponseDto
        (
        string Name,
        Guid AccountId, 
        string AccountNumber,
        decimal Balance
        );
}
