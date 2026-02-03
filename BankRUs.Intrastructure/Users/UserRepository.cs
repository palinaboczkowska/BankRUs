using BankRUs.Application.Abstractions;
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
}