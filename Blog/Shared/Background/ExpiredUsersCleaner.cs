using Blog.Domain.Entities;
using Blog.Domain.Repositories.Users;

namespace Blog.Shared.Background;

public class ExpiredUsersCleaner(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken);

            using var scope = serviceProvider.CreateScope();
            var unvalidatedUsersRepo = scope.ServiceProvider.GetRequiredService<IUnvalidatedUsersRepo>();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepo>();

            List<string> expiredUsers = await userRepo.RemoveUnverifiedUsers();
            await unvalidatedUsersRepo.RemoveExpiredValidationCodes(expiredUsers);
        }
    }
}