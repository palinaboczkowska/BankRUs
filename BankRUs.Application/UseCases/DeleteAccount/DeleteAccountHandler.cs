using BankRUs.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.DeleteAccount
{
    public sealed class DeleteAccountHandler
    {
        private readonly IUserRepository _repo;

        public DeleteAccountHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(DeleteAccountCommand command)
        {
            await _repo.DeleteUserAsync(command.UserId);
        }
    }

}
