using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.UpdateAccountDetails
{
    public sealed record UpdateAccountDetailsCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    string? Email,
    string? SocialSecurityNumber
);

}
