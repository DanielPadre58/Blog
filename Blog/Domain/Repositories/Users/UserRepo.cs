using Blog.Application.Dtos.Users;
using Blog.DbContext;
using Blog.Domain.Entities;
using Blog.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Users;

public class UserRepo(BlogContext context) : IUserRepo
{
    public async Task<User> Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task Delete(string username)
    {
        var user = await GetByUsername(username);

        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<User> Edit(string username, UserUpdateDto updatedUser)
    {
        var user = await GetByUsername(username);

        if (updatedUser.Username != null)
            user.Username = updatedUser.Username;
        if (updatedUser.FirstName != null)
            user.FirstName = updatedUser.FirstName;
        if (updatedUser.LastName != null)
            user.LastName = updatedUser.LastName;
        if (updatedUser.Birthday != null)
            user.Birthday = updatedUser.Birthday;

        await context.SaveChangesAsync();

        return user;
    }
    
    public async Task<User> GetByUsername(string username)
    {
        var user = await context.Users
                       .FirstOrDefaultAsync(u => u.Username == username)??
                   throw new NotFoundException($"User with username {username} not found");

        return user;
    }

    public async Task<bool> UsernameExists(string username)
    {
        return await context.Users
            .AnyAsync(u => u.Username == username);
    }

    public async Task<List<User>> GetByUsernameUncapitalized(string username)
    {
        return await context.Users
            .Where(u => u.Username.ToLower() == username)
            .ToListAsync();
    }

    public Task<User> VerifyUser(string username)
    {
        var user = context.Users.FirstOrDefaultAsync(u => u.Username == username);

        user.Result.Verify();
        context.SaveChangesAsync();

        return user;
    }
}