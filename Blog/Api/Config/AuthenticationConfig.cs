using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Api.Config;

public class AuthenticationConfig
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;
    
    public AuthenticationConfig(IServiceCollection services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
        
        AddAuthentication();
    }

    private void AddAuthentication()
    {
        _services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["AppSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["AppSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]!)),
                    ValidateIssuerSigningKey = true
                };
        
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = (context) =>
                    {
                        Console.WriteLine("Token validated successfully.");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine($"OnChallenge: {context.Error}, {context.ErrorDescription}");
                        return Task.CompletedTask;
                    }
                };
            });
    }
}