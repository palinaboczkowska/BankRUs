using BankRUs.Application.UseCases.GetUsers;

namespace BankRUs.Application.Abstractions;

public interface IUserRepository
{
    Task<int> CountAsync();
    Task<IReadOnlyList<UserResult>> GetPagedAsync(int page, int pageSize);
}