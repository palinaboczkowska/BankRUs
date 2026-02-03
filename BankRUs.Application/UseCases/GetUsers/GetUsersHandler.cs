using BankRUs.Api.Configuration;
using BankRUs.Application.Abstractions;
using Microsoft.Extensions.Options;
namespace BankRUs.Application.UseCases.GetUsers;

public class GetUsersHandler
{
    private readonly IUserRepository _repo;
    private readonly QueryParamsOptions _options;

    public GetUsersHandler(IUserRepository repo, IOptions<QueryParamsOptions> options)
    {
        _repo = repo;
        _options = options.Value;
    }

    public async Task<GetUsersResult> Handle(GetUsersQuery query)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = Math.Min(query.PageSize, _options.MaxPageSize);

        var totalItems = await _repo.CountAsync();
        var users = await _repo.GetPagedAsync(page, pageSize);

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new GetUsersResult(
            Data: users,
            Page: page,
            PageSize: pageSize,
            TotalItems: totalItems,
            TotalPages: totalPages
        );
    }
}