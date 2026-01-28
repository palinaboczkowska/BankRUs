using BankRUs.Api.Dtos.BankAccounts;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Application.UseCases.OpenBankAccount;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers
{
    [ApiController]
    [Route("api/bank-accounts")]
    public class BankAccountsController : ControllerBase
    {
        private readonly OpenBankAccountHandler _handler;

        public BankAccountsController(OpenBankAccountHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> OpenBankAccount(CreateBankAccountRequestDto request)
        {
            var command = new OpenBankAccountCommand(
                UserId: request.UserId,
                Name: request.Name
            );

            var result = await _handler.HandleAsync(command);

            var response = new BankAccountResponseDto(
                AccountId: result.Id,
                AccountNumber: result.AccountNumber,
                Name: result.Name,
                Balance: result.Balance
            );

            return Created(string.Empty, response);
        }
    }
}

