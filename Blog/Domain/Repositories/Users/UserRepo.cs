using Blog.Application.Dtos.Users;
using Blog.DbContext;
using Blog.Domain.Entities;
using Blog.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Domain.Repositories.Users;

public class UserRepo(BlogContext context) : IUserRepo
{
    public async Task<User> CreateAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task DeleteAsync(string username)
    {
        var user = await GetByUsernameAsync(username);

        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<User> EditAsync(string username, UserUpdateDto updatedUser)
    {
        var user = await GetByUsernameAsync(username);

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

    public async Task<User> GetByUsernameAsync(string username)
    {
        var user = await context.Users
                       .FirstOrDefaultAsync(u => u.Username == username) ??
                   throw new NotFoundException($"User with username {username} not found");

        return user;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await context.Users
            .AnyAsync(u => u.Username == username);
    }

    public async Task<List<User>> GetByUsernameUncapitalizedAsync(string username)
    {
        return await context.Users
            .Where(u => u.Username.ToLower() == username)
            .ToListAsync();
    }

    public async Task<User> VerifyUserAsync(string username)
    {
        var user = await GetByUsernameAsync(username);

        user.Verify();
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<List<string>> RemoveUnverifiedUsersAsync()
    {
        var expirationDate = DateTime.UtcNow.AddDays(-7);

        var unverifiedUsers = await context.Users
            .Where(u => u.CreatedAt < expirationDate && !u.IsVerified)
            .ToListAsync();

        context.Users.RemoveRange(unverifiedUsers);
        await context.SaveChangesAsync();

        Console.WriteLine($"Removed {unverifiedUsers.Count} unverified users.");
        
        return unverifiedUsers.Select(u => u.Username).ToList();
    }
}