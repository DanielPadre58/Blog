using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Resend;

namespace Blog.Application.External_Services;

public class SmptService(IConfiguration config) : ISmptService
{
    public async Task SendVerificationCodeAsync(string email, string code)
    {
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress ("BlogApi", config["Smtp:Username"]));
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
            config["Smtp:Host"],
            int.Parse(config["Smtp:Port"]),
            SecureSocketOptions.SslOnConnect);
        
        await client.AuthenticateAsync(
            config["Smtp:Username"], 
            config["Smtp:Password"]);
        
        await client.SendAsync(message);
        
        await client.DisconnectAsync(true);
    }
}