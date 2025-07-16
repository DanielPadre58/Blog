using Blog.Application.Dtos.User;
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

    public Task EditById(int id, UserUpdateDto updatedUser)
    {
        var user = GetById(id).Result;
        
        if(updatedUser.Username != null)
            user.Username = updatedUser.Username;
        if(updatedUser.FirstName != null)
            user.FirstName = updatedUser.FirstName;
        if(updatedUser.LastName != null)
            user.LastName = updatedUser.LastName;
        if(updatedUser.Birthday != null)
            user.Birthday = updatedUser.Birthday;
        
        return context.SaveChangesAsync();
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