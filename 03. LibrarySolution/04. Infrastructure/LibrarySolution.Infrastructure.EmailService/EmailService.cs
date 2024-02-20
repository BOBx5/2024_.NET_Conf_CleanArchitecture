using LibrarySolution.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace LibrarySolution.Infrastructure.EmailService;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"이메일 아무튼 처리됨 (""{email}""/""{subject})""", email, subject);
        return Task.CompletedTask;
    }
}
