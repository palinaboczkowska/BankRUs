using BankRUs.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.UpdateAccountDetails
{
    public sealed class UpdateAccountDetailsHandler
    {
        private readonly IUserRepository _userRepository;

        public UpdateAccountDetailsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateAccountDetailsCommand command)
        {
            await _userRepository.UpdateAccountDetailsAsync(
                command.UserId,
                command.FirstName,
                command.LastName,
                command.Email,
                command.SocialSecurityNumber
            );
        }
    }


}
