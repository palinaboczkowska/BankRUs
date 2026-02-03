namespace BankRUs.Application.UseCases.GetUsers;

public record GetUsersResult(
    IReadOnlyList<UserResult> Data,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);

public record UserResult(
    string Id,
    string FirstName,
    string LastName,
    string Email
);