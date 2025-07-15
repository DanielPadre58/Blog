using Blog.Domain.Entities;
using Blog.Persistence.DbContext;

namespace Blog.Persistence.Repositories;

public class UserRepo(BlogContext context) : IUserRepo
{
    public async Task Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}