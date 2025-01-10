namespace Domain;

public interface IEmailSender
{
    public Task SendAsync(string email, string subject, string htmlMessage);
    public Task SendManyAsync(IEnumerable<string> emails, string subject, string htmlMessage);
}
