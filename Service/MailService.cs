using MimeKit;
using MailKit.Net.Smtp;

namespace Service;

public static class MailService
{
    private static MimeMessage CreateEmailMessage(string subject, string body, IEnumerable<string> receiverEmails, string receiverFullname = "")
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Learning Platform", Environment.GetEnvironmentVariable("COMPANY_MAIL")));
        message.Subject = subject;

        foreach (var email in receiverEmails)
        {
            message.To.Add(new MailboxAddress(receiverFullname, email));
        }

        message.Body = new TextPart("html") { Text = body };
        return message;
    }

    private static void SendEmail(MimeMessage message)
    {
        using var client = new SmtpClient();
        try
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(Environment.GetEnvironmentVariable("COMPANY_MAIL"), Environment.GetEnvironmentVariable("G_PASS"));
            client.Send(message);
        }
        catch (SmtpCommandException ex)
        {
            
            throw new InvalidOperationException($"Error sending email: SMTP command failed - {ex.Message}");
        }
        catch (SmtpProtocolException ex)
        {
            
            throw new InvalidOperationException($"Error sending email: SMTP protocol error - {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error sending email: {ex.Message}");
        }
        finally
        {
            client.Disconnect(true);
        }
    }

    public static void SendEmail(string receiverFullname, string receiverEmail, string body)
    {
        var message = CreateEmailMessage("Welcome To Learning Platform", body, new[] { receiverEmail }, receiverFullname);
        SendEmail(message);
    }

    public static void SendVerificationEmail(string email, string token)
    {
        var verificationLink = $"https://learning-platform-80506.web.app/verify-email?token={token}";
        string body = $"<p>Please verify your email by clicking on the link below:</p><p><a href='{verificationLink}'>Verify Email</a></p>";
        var message = CreateEmailMessage("Please verify your email", body, new[] { email });
        SendEmail(message);
    }

    public static void SendPasswordResetEmail(string email, string token)
    {
        var resetPasswordLink = $"https://learning-platform-80506.web.app/reset-password?token={token}";
        string body = $"<p>You have requested to reset your password. Please click the link below to reset it:</p><p><a href='{resetPasswordLink}'>Reset Password</a></p>";
        var message = CreateEmailMessage("Password Reset Request", body, new[] { email });
        SendEmail(message);
    }

    public static void SendEmailToMultipleRecipients(List<string> receiverEmails, string subject, string body)
    {
        var message = CreateEmailMessage(subject, body, receiverEmails);
        SendEmail(message);
    }
}
