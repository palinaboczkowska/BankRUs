using BankRUs.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace BankRUs.Intrastructure.Email;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var host = _config["Smtp:Host"] ?? "localhost";
        var port = int.Parse(_config["Smtp:Port"] ?? "25");
        var from = _config["Smtp:From"] ?? "no-reply@bankrus.local";

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = true
        };

        var mail = new MailMessage(from, to, subject, body);

        await client.SendMailAsync(mail);
    }
}