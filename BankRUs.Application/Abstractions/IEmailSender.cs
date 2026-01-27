namespace BankRUs.Application.Abstractions
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body);
    }
}
