namespace BankRUs.Api.Dtos.Users
{
    public record PagedResponse<T>(
    IReadOnlyList<T> Data,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);

}
