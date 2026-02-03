using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.OpenBankAccount
{
    public record OpenBankAccountResult(
    Guid Id,
    string AccountNumber,
    string Name,
    decimal Balance,
    string UserId
);

}
