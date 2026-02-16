using BankRUs.Api.Dtos.Transactions;
using BankRUs.Application.Abstractions;
using BankRUs.Application.UseCases.DepositMoney;
using BankRUs.Application.UseCases.GetTransactions;
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
    private readonly GetTransactionsHandler _transactionsHandler;

    public TransactionsController(DepositMoneyHandler depositHandler, WithdrawMoneyHandler withdrawHandler, GetTransactionsHandler transactionsHandler )
    {
        _depositHandler = depositHandler;
        _withdrawHandler = withdrawHandler;
        _transactionsHandler = transactionsHandler;
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

    [HttpGet("{accountId:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(
    Guid accountId,
    [FromQuery] GetTransactionsQuery query)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var command = new GetTransactionsCommand(
            userId!,
            accountId,
            query.Page,
            query.PageSize,
            query.From,
            query.To,
            query.Type,
            query.Sort
        );
        try
        {
            var (account, items, totalCount) = await _transactionsHandler.Handle(command);
            var response = new TransactionListResponse
            {
                AccountId = account.Id,
                Currency = "SEK",
                Balance = account.Balance,
                Paging = new PagingInfo
                {
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
                },
                Items = items.Select(t => new TransactionDto(
                    TransactionId: t.Id,
                    Amount: t.Amount,
                    Reference: t.Reference,
                    CreatedAt: t.CreatedAt,
                    Type: t.Type.ToString().ToLower()
                )).ToList()
            };
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

}