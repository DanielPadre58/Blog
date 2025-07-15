using Blog.Domain.Entities;
using Blog.Persistence.DbContext;

namespace Blog.Persistence.Repositories;

public class UserRepo(BlogContext context) : IUserRepo
{
    public async Task<User>? Create(User user)
    {
        try
        {
            var newUser = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            
            return newUser.Entity;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
