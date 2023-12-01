using MimeKit;

namespace Service;

public static class MailService
{
    public static void SendEmail(string receiverFullname, string receiverEmail, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Learning Platform", Environment.GetEnvironmentVariable("COMPANY_MAIL")));
        message.To.Add(new MailboxAddress(receiverFullname, receiverEmail));
        message.Subject = "Welcome To Learning Platform";

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(Environment.GetEnvironmentVariable("COMPANY_MAIL"), Environment.GetEnvironmentVariable("G_PASS"));
            client.Send(message);
            client.Disconnect(true);
        }
    }
    
    public static void SendVerificationEmail(string email, string token)
    {
        // Use the localhost address in the link.
        var verificationLink = $"http://localhost:5000/api/account/verify-email?token={token}";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Learning Platform", Environment.GetEnvironmentVariable("COMPANY_MAIL")));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Please verify your email";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"<p>Please verify your email by clicking on the link below:</p><p><a href='{verificationLink}'>Verify Email</a></p>"
        };
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(Environment.GetEnvironmentVariable("COMPANY_MAIL"), Environment.GetEnvironmentVariable("G_PASS"));
            client.Send(message);
            client.Disconnect(true); 
        } 
    }
    public static void SendPasswordResetEmail(string email, string token)
    {
        var resetPasswordLink = $"http://localhost:5000/api/account/reset-password?token={token}";
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Learning Platform", Environment.GetEnvironmentVariable("COMPANY_MAIL")));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Password Reset Request";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"<p>You have requested to reset your password. Please click the link below to reset it:</p><p><a href='{resetPasswordLink}'>Reset Password</a></p>"
        };
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(Environment.GetEnvironmentVariable("COMPANY_MAIL"), Environment.GetEnvironmentVariable("G_PASS"));
            client.Send(message);
            client.Disconnect(true);
        }
    }
    
}