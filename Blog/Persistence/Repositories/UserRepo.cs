using Blog.Domain.Entities;
using Blog.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Repositories;

public class UserRepo(BlogContext context) : IUserRepo
{
    public async Task Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<User> GetById(int id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id) ??
               throw new NullReferenceException();
    }
}