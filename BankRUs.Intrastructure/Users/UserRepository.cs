using BankRUs.Application.Abstractions;
using BankRUs.Application.UseCases.GetSingleUser;
using BankRUs.Application.UseCases.GetUsers;
using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Users;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<int> CountAsync()
        => await _db.Users.CountAsync();

    public async Task<IReadOnlyList<UserResult>> GetPagedAsync(int page, int pageSize)
        => await _db.Users
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserResult(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email
            ))
            .ToListAsync();

    public async Task<GetUserByIdResult?> GetByIdWithAccountsAsync(string id)
    {
        var user = await _db.Users
            .Include(u => u.BankAccounts)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return null;

        return new GetUserByIdResult(
            Id: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email,
            BankAccounts: user.BankAccounts
                .Select(a => new BankAccountResult(
                    Id: a.Id,
                    AccountNumber: a.AccountNumber,
                    Name: a.Name,
                    Balance: a.Balance
                ))
                .ToList()
        );
    }
}