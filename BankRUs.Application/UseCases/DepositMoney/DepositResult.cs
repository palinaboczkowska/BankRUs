using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.DepositMoney
{
    public sealed record DepositResult(
    Guid TransactionId,
    string UserId,
    string Type,
    decimal Amount,
    string Currency,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);

}
