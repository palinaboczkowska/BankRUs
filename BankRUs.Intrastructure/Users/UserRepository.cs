using BankRUs.Application.Abstractions;
using BankRUs.Application.Common;
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

    public async Task<PagedResult<UserResult>> SearchAsync(int page, int pageSize, string ssn)
    {
        var query = _db.Users
            .Where(u => u.SocialSecurityNumber.StartsWith(ssn))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName);

        var totalItems = await query.CountAsync();

        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserResult(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email
            ))
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedResult<UserResult>(
            Data: users,
            Page: page,
            PageSize: pageSize,
            TotalItems: totalItems,
            TotalPages: totalPages
        );
    }

}