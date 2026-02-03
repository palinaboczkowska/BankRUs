using BankRUs.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Intrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string SocialSecurityNumber { get; set; }
    public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
