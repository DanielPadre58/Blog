using Blog.Domain.Entities;
using Blog.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Repositories.Users;

public class UserRepo(BlogContext context) : IUserRepo
{
    public async Task Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await context.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<User> GetById(int id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id) ??
               throw new NullReferenceException();
    }
    
    public async Task<bool> UsernameExists(string username)
    {
        return await context.Users.AnyAsync(u => u.Username == username);
    }
    
    public async Task<List<User>> GetByUsernameUncapitalized(string username)
    {
        return await context.Users.Where(u => u.Username.ToLower() == username).ToListAsync() ??
               throw new NullReferenceException();
    }
}