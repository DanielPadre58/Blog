namespace Blog.Application.External_Services;

public interface ISmptService
{
    public Task SendVerificationCodeAsync(string email, string code);
}