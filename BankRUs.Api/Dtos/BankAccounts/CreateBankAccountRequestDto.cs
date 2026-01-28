namespace BankRUs.Api.Dtos.BankAccounts
{
    public record CreateBankAccountRequestDto(
            Guid UserId,
            string Name
        );

}
