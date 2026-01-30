namespace BankRUs.Application;

public sealed record Token(
  string AccessToken,
  DateTime ExpiresAtUtc
);
