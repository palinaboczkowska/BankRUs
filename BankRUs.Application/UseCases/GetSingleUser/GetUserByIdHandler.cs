using BankRUs.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.GetSingleUser
{
    public class GetUserByIdHandler
    {
        private readonly IUserRepository _repo;

        public GetUserByIdHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetUserByIdResult?> Handle(GetUserByIdQuery query)
        {
            return await _repo.GetByIdWithAccountsAsync(query.Id);
        }
    }

}
