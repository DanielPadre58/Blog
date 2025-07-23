using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.Application.Services.Authentication;

public interface IAuthenticationService
{
    public Task<string> AuthenticateAsync(LoginDto loginData);
    public string GenerateJwtTokenAsync(User user);
}