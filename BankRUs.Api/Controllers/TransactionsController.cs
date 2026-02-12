using BankRUs.Api.Dtos.Transactions;
using BankRUs.Application.Abstractions;
using BankRUs.Application.UseCases.DepositMoney;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;


namespace BankRUs.Api.Controllers;

[ApiController]
[Route("api/bank-accounts")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly DepositMoneyHandler _handler;

    public TransactionsController(DepositMoneyHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("{accountId:guid}/deposits")]
    public async Task<IActionResult> Deposit(
    [FromRoute] Guid accountId,
    [FromBody] DepositRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new DepositMoneyCommand(
            userId,
            accountId,
            request.Amount,
            request.Reference
        );

        var result = await _handler.Handle(command);

        return Ok(result);
    }

}