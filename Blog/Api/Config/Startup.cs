using Blog.Api.Config;
using Blog.Domain.DbContext;
using Blog.Shared.Background;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<BlogContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
                         throw new InvalidOperationException(
                             "Missing configuration: Default database connection string"));
});

builder.Services.AddControllers();

var authenticationConfig = new AuthenticationConfig(builder.Services, builder.Configuration);
var swaggerConfig = new SwaggerConfig(builder.Services);
var dependencyInjector = new DependencyInjector(builder.Services, builder.Configuration);

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