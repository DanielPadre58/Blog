using Blog.Application.External_Services;
using Blog.Application.External_Services.Smtp;
using Blog.Application.Services.Authentication;
using Blog.Application.Services.Comments;
using Blog.Application.Services.Posts;
using Blog.Application.Services.Users;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Comments;
using Blog.Domain.Repositories.Posts;
using Blog.Domain.Repositories.Redis;
using Blog.Domain.Repositories.Tags;
using Blog.Domain.Repositories.Users;
using Blog.Shared.Security;
using Blog.Shared.Validation;
using StackExchange.Redis;

namespace Blog.Api.Config;

public class DependencyInjector
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;
    
    public DependencyInjector(IServiceCollection services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
        
        AddScoped();
        AddSingleton();
    }

    private void AddScoped()
    {
        _services.AddScoped<IUserRepo, UserRepo>();
        _services.AddScoped<IUserService, UserService>();
        _services.AddScoped<IPostRepo, PostRepo>();
        _services.AddScoped<IPostService, PostService>();
        _services.AddScoped<IPasswordHasher, PasswordHasher>();
        _services.AddScoped<ISmptService, SmptService>();
        _services.AddScoped<IUnvalidatedUsersRepo, UnvalidatedUsersRepo>();
        _services.AddScoped<IAuthenticationService, AuthenticationService>();
        _services.AddScoped<IRedisRepo, RedisRepo>();
        _services.AddScoped<ITagRepo, TagRepo>();
        _services.AddScoped<ICommentService, CommentService>();
        _services.AddScoped<ICommentRepo, CommentRepo>();
        _services.AddScoped<IValidator, Validator>();
        _services.AddScoped<User>();
    }
    
    private void AddSingleton()
    {
        _services.AddSingleton<IConnectionMultiplexer>(opt =>
            ConnectionMultiplexer.Connect(_configuration.GetConnectionString("DockerRedisConnection") ??
                                          throw new InvalidOperationException(
                                              "Missing configuration: Redis database connection string")));
    }
}