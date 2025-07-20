namespace Blog.Application.External_Services;

public interface ISmtpService
{
    public Task SendVerificationCodeAsync(string email, string code);
}