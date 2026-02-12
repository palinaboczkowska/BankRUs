using BankRUs.Api.Dtos.Transactions;
using BankRUs.Application.Abstractions;
using BankRUs.Application.UseCases.DepositMoney;
using BankRUs.Application.UseCases.WithdrawMoney;
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
    private readonly DepositMoneyHandler _depositHandler;
    private readonly WithdrawMoneyHandler _withdrawHandler;

    public TransactionsController(DepositMoneyHandler depositHandler, WithdrawMoneyHandler withdrawHandler )
    {
        _depositHandler = depositHandler;
        _withdrawHandler = withdrawHandler;
    }

    [HttpPost("{accountId:guid}/deposits")]
    public async Task<IActionResult> Deposit(
    [FromRoute] Guid accountId,
    [FromBody] DepositRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new DepositMoneyCommand(
            userId!,
            accountId,
            request.Amount,
            request.Reference
        );

        var result = await _depositHandler.Handle(command);

        var response = new TransactionResponse
        {
            TransactionId = result.Id,
            AccountId = result.BankAccountId,
            Type = result.Type.ToString().ToLower(),
            Amount = result.Amount,
            Currency = "SEK",
            Reference = result.Reference,
            CreatedAt = result.CreatedAt,
            BalanceAfter = result.BalanceAfter
        };

        return Created("", response);
    }

    [HttpPost("{accountId:guid}/withdrawals")]
    public async Task<IActionResult> Withdraw(
    Guid accountId,
    [FromBody] WithdrawRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new WithdrawMoneyCommand(
            userId!,
            accountId,
            request.Amount,
            request.Reference
        );

        try
        {
            var result = await _withdrawHandler.Handle(command);

            var response = new TransactionResponse
            {
                TransactionId = result.Id,
                AccountId = result.BankAccountId,
                Type = result.Type.ToString().ToLower(),
                Amount = result.Amount,
                Currency = "SEK",
                Reference = result.Reference,
                CreatedAt = result.CreatedAt,
                BalanceAfter = result.BalanceAfter
            };

            return Created("", response);

        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                type = "https://httpstatuses.com/409",
                title = "Insufficient funds",
                status = 409,
                detail = ex.Message
            });
        }
    }

}