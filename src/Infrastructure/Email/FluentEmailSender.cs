using Application.Abstractions;
using FluentEmail.Core;

namespace Infrastructure.Email;

public class FluentEmailSender(
    IFluentEmail _fluentEmail
) : IEmailSender
{
    public async Task SendAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine($"Sending email to {email}...");

        try
        {
            await _fluentEmail
                .To(email)
                .Subject(subject)
                .Body(htmlMessage, isHtml: true)
                .SendAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
        }
    }

    public async Task SendManyAsync(IEnumerable<string> emails, string subject, string htmlMessage)
    {
        foreach (var email in emails)
            await SendAsync(email, subject, htmlMessage);
    }
}
