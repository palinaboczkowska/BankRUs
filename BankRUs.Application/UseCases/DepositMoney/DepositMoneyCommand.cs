using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.DepositMoney
{
    public sealed record DepositMoneyCommand(
    string UserId,
    Guid BankAccountId,
    decimal Amount,
    string? Reference
);

}
