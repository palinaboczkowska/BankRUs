using BankRUs.Api.Dtos.Accounts;
using BankRUs.Api.Dtos.BankAccounts;
using BankRUs.Application.Abstractions;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Intrastructure.Persistance;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BankRUs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly OpenAccountHandler _openAccountHandler;
    private readonly IBankAccountRepository _accountRepository;

    public AccountsController(OpenAccountHandler openAccountHandler, IBankAccountRepository accountRepository)
    {
        _openAccountHandler = openAccountHandler;
        _accountRepository = accountRepository;
    }

    // POST /api/accounts (Endpoint /  API endpoint)
    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountRequestDto request)
    {
        // Tjocka vs Tunna controllers
        try
        {
            var openAccountResult = await _openAccountHandler.HandleAsync(
            new OpenAccountCommand(
                FirstName: request.FirstName,
                LastName: request.LastName,
                SocialSecurityNumber: request.SocialSecurityNumber,
                Email: request.Email));

        var response = new CreateAccountResponseDto(openAccountResult.UserId, 
            openAccountResult.AccountId,
            openAccountResult.AccountNumber);

        // Returnera 201 Created
        return Created(string.Empty, response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<IEnumerable<BankAccountResponseDto>>> GetAccounts(Guid userId)
    {
        var accounts = await _accountRepository.GetByUserIdAsync(userId);

        var result = accounts.Select(a =>
            new BankAccountResponseDto(a.Name, a.Id, a.AccountNumber, a.Balance)
        );

        return Ok(result);
    }

    private static bool IsValidLuhn(string digits)
    {
        var sum = 0;

        for (int i = 0; i < 9; i++)
        {
            var num = digits[i] - '0';
            num *= (i % 2 == 0) ? 2 : 1;
            if (num > 9) num -= 9;
            sum += num;
        }

        var controlDigit = (10 - (sum % 10)) % 10;

        return controlDigit == digits[9] - '0';
    }

}
