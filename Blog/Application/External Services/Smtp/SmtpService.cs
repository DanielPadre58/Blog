using System.Net.Mail;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Blog.Application.External_Services;

public class SmtpService(IConfiguration configuration) : ISmtpService
{
    public async Task SendVerificationCodeAsync(string email, string code)
    {
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress ("BlogApi", configuration["Smtp:From"]));
        message.To.Add (new MailboxAddress ("User", email));
        message.Subject = "Verification Code";

        message.Body = new TextPart ("plain") {
            Text = @"Your verification code is: " + code +
                   "\n\nNote: This code is valid for 10 minutes."
        };
        
        await SendMessageAsync(message);
    }

    private async Task SendMessageAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        
        await client.ConnectAsync(
            configuration["Smtp:Host"],
            int.Parse(configuration["Smtp:Port"]),
            SecureSocketOptions.None);
        
        await client.SendAsync(message);
        
        await client.DisconnectAsync(true);
    }
}