using Blog.Application.Dtos.Authentication;
using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.Application.Services.Authentication;

public interface IAuthenticationService
{
    public Task<string> AuthenticateAsync(LoginDto loginData);
    public string GenerateJwtToken(User user);
    public Task<string> GenerateRefreshTokenAsync(string username);
    public Task VerifyRefreshTokenAsync(string username, string refreshToken);
}