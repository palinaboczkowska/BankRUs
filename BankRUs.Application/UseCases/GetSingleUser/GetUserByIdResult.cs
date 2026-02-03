using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.GetSingleUser
{
    public record BankAccountResult(
     Guid Id,
     string AccountNumber,
     string Name,
     decimal Balance
 );

    public record GetUserByIdResult(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        IReadOnlyList<BankAccountResult> BankAccounts
    );

}
