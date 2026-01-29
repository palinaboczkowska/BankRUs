using BankRUs.Application.Abstractions;
using BankRUs.Application.Identity;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenAccount;

public class OpenAccountHandler
{
    private readonly IIdentityService _identityService;
    private readonly IBankAccountRepository _accountRepository;
    private readonly IEmailSender _emailSender;
    private readonly TestPersonnummerValidator _validator;



    public OpenAccountHandler(IIdentityService identityService, IBankAccountRepository accountRepository, IEmailSender emailSender, TestPersonnummerValidator validator)
    {
        _identityService = identityService;
        _accountRepository = accountRepository;
        _emailSender = emailSender;
        _validator = validator;
    }

    public async Task<OpenAccountResult> HandleAsync(OpenAccountCommand command)
    {
        // TODO: Skapa användarkonto (ASP.NET Core Identity)
        // Delegera till infrastructure
        if (!await _validator.IsValidAsync(command.SocialSecurityNumber))
        {
            throw new InvalidOperationException("Ogiltigt personnummer enligt Skatteverket.");
        }

        var createUserResult = await _identityService.CreateUserAsync(new CreateUserRequest(
            FirstName: command.FirstName,
            LastName: command.LastName,
            SocialSecurityNumber: command.SocialSecurityNumber,
            Email: command.Email
         ));

        if (!createUserResult.Succeeded || !createUserResult.UserId.HasValue)
        {
            throw new InvalidOperationException(
                $"Unable to create user: {string.Join(", ", createUserResult.Errors)}"
            );
        }

        // TODO: SocialSecurityNumber + Email ska vara UNIQUE

        // TODO: Skapa bankkonto
        // Delegera till infrastructure
        var userId = createUserResult.UserId.Value;
        var accountNumber = GenerateAccountNumber();
        var account = new BankAccount(
                userId: userId,
                accountNumber: accountNumber,
                name: "Standardkonto"
        );


        await _accountRepository.AddAsync(account);


        // TODO: Skicka välkomstmail till kund
        // Delegera till infrastructure
        // _emailSender.Send("Ditt bankkonto är nu redo!");
        await _emailSender.SendAsync(
        command.Email,
        "Ditt bankkonto är nu redo!",
        $"Hej {command.FirstName}! Ditt konto {accountNumber} är nu öppnat.");

        return new OpenAccountResult(
            UserId: userId,
            AccountId: account.Id,
            AccountNumber: accountNumber);

    }
    private string GenerateAccountNumber()
    {
        return DateTime.UtcNow.Ticks.ToString();
    }


}