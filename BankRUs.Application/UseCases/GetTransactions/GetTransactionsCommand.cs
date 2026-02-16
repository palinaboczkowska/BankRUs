public sealed record GetTransactionsCommand(
    string UserId,
    Guid AccountId,
    int Page,
    int PageSize,
    DateTime? From,
    DateTime? To,
    string? Type,
    string Sort
);