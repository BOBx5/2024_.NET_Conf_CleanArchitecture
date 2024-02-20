namespace LibrarySolution.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string htmlMessage, CancellationToken cancellationToken);
}