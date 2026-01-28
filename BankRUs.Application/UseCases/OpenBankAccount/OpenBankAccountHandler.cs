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
        private readonly IBankAccountRepository _accountRepository;

        public OpenBankAccountHandler(IBankAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<OpenBankAccountResult> HandleAsync(OpenBankAccountCommand command)
        {
            var accountNumber = GenerateAccountNumber();
            var account = new BankAccount(
                userId: command.UserId,
                accountNumber: accountNumber,
                name: command.Name
            );


            await _accountRepository.AddAsync(account);

            return new OpenBankAccountResult(
                     Id: account.Id,
                     AccountNumber: account.AccountNumber,
                     Name: account.Name,
                     Balance: account.Balance,
                     UserId: account.UserId
            );


        }

        private string GenerateAccountNumber()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }
    }
}
