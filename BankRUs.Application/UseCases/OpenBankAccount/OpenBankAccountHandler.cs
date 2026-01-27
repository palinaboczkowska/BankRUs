using BankRUs.Application.Abstractions;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.OpenBankAccount
{
    public class OpenBankAccountHandler
    {
        private readonly IAccountRepository _accountRepository;

        public OpenBankAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<OpenAccountResult> HandleAsync(OpenBankAccountCommand command)
        {
            var accountNumber = GenerateAccountNumber();
            var account = new BankAccount(command.UserId, accountNumber);

            await _accountRepository.AddAsync(account);

            return new OpenAccountResult(
                UserId: command.UserId,
                AccountId: account.Id,
                AccountNumber: accountNumber);
        }

        private string GenerateAccountNumber()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }
    }
}
