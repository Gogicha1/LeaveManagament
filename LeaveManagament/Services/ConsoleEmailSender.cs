using Microsoft.AspNetCore.Identity.UI.Services;

namespace LeaveManagament.Services
{
    // Used in Development so registration/password-reset flows don't depend on a real SMTP
    // server being available on localhost. Emails are just logged instead of sent.
    public class ConsoleEmailSender(ILogger<ConsoleEmailSender> logger) : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            logger.LogInformation(
                "Email suppressed in Development.\nTo: {Email}\nSubject: {Subject}\nBody:\n{Body}",
                email, subject, htmlMessage);
            return Task.CompletedTask;
        }
    }
}
