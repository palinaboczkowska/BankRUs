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
        public async Task<ActionResult<OpenAccountResult>> OpenBankAccount(OpenBankAccountCommand command)
        {
            var result = await _handler.HandleAsync(command);
            return Created(string.Empty, result);
        }
    }
}
