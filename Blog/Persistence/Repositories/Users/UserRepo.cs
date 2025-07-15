using Blog.Persistence.DbContext;
using Blog.Persistence.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Repositories.Users;

public class UserRepo(BlogContext context) : IUserRepo
{
    public async Task Create(Domain.Entities.User user)
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

    public async Task<Domain.Entities.User> GetById(int id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id) ??
               throw new NullReferenceException();
    }
    
    public async Task<List<Domain.Entities.User>> GetByUsername(string username)
    {
        return await context.Users.Where(u => u.Username == username).ToListAsync() ??
               throw new NullReferenceException();
    }
    
    public async Task<List<Domain.Entities.User>> GetByUsernameUncapitalized(string username)
    {
        return await context.Users.Where(u => u.Username.ToLower() == username).ToListAsync() ??
               throw new NullReferenceException();
    }
}