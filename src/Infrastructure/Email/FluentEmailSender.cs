using Application.Abstractions;
using FluentEmail.Core;

namespace Infrastructure.Email;

public class FluentEmailSender(
    IFluentEmail _fluentEmail
) : IEmailSender
{
    public async Task SendAsync(string email, string subject, string htmlMessage)
    {
        await _fluentEmail
            .To(email)
            .Subject(subject)
            .Body(htmlMessage, isHtml: true)
            .SendAsync();
    }

    public async Task SendManyAsync(IEnumerable<string> emails, string subject, string htmlMessage)
    {
        foreach (var email in emails)
            await SendAsync(email, subject, htmlMessage);
    }
}
