using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Application.Dtos.Authentication;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Redis;
using Blog.Domain.Repositories.Users;
using Blog.Shared.Exceptions;
using Blog.Shared.Security;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Application.Services.Authentication;

public class AuthenticationService(
    IUserRepo repository,
    IPasswordHasher passwordHasher,
    IRedisRepo redisRepository,
    IConfiguration configuration) : IAuthenticationService
{
    public async Task<string> AuthenticateAsync(LoginDto loginData)
    {
        try
        {
            var user = await repository.GetByUsernameAsync(loginData.Username);

            if (!passwordHasher.PasswordEquals(user, loginData.Password))
                throw new InvalidAuthenticationData();

            return GenerateJwtToken(user);
        }
        catch (NotFoundException)
        {
            throw new InvalidAuthenticationData();
        }
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["AppSettings:Token"] ??
                                   throw new InvalidOperationException("Missing configuration: JWT token secret key")));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(10);

        var token = new JwtSecurityToken(
            issuer: configuration["AppSettings:Issuer"],
            audience: configuration["AppSettings:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(string username)
    {
        await repository.GetByUsernameAsync(username);

        var refreshToken = Guid.NewGuid().ToString();
        await redisRepository.SetRefreshTokenAsync(username, refreshToken);

        return refreshToken;
    }

    public async Task VerifyRefreshTokenAsync(string username, string refreshToken)
    {
        if (refreshToken != await redisRepository.GetRefreshTokenAsync(username))
            throw new InvalidAuthenticationData("Invalid refresh token.");
    }
}
    