using System.Text;
using Blog.Application.External_Services;
using Blog.Application.External_Services.Smtp;
using Blog.Application.Services.Authentication;
using Blog.Application.Services.Comments;
using Blog.Application.Services.Posts;
using Blog.Application.Services.Users;
using Blog.Domain.DbContext;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Repositories.Comments;
using Blog.Domain.Repositories.Posts;
using Blog.Domain.Repositories.Redis;
using Blog.Domain.Repositories.Tags;
using Blog.Domain.Repositories.Users;
using Blog.Shared.Background;
using Blog.Shared.Security;
using Blog.Shared.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    
    c.MapType<PostFilter>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(PostFilter))
            .Select(n => new OpenApiString(n))
            .Cast<IOpenApiAny>()
            .ToList()
    });
    
    c.MapType<UserPostsFilter>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(UserPostsFilter))
            .Select(n => new OpenApiString(n))
            .Cast<IOpenApiAny>()
            .ToList()
    });
});

builder.Services.AddDbContext<BlogContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
                         throw new InvalidOperationException(
                             "Missing configuration: Default database connection string"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("DockerRedisConnection") ??
                                  throw new InvalidOperationException(
                                      "Missing configuration: Redis database connection string")));

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
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

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostRepo, PostRepo>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ISmptService, SmptService>();
builder.Services.AddScoped<IUnvalidatedUsersRepo, UnvalidatedUsersRepo>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IRedisRepo, RedisRepo>();
builder.Services.AddScoped<ITagRepo, TagRepo>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();
builder.Services.AddScoped<IValidator, Validator>();
builder.Services.AddScoped<User>();

builder.Services.AddHostedService<ExpiredUsersCleaner>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API v1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();