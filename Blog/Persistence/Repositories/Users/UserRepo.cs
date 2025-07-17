using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;
using Blog.Persistence.DbContext;
using Blog.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Repositories.Users;

public class UserRepo(BlogContext context) : IUserRepo
{
    public async Task<User> Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task Delete(int id)
    {
        var user = await GetById(id);

        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<User> EditById(int id, UserUpdateDto updatedUser)
    {
        var user = await GetById(id);

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

    public async Task<User> GetById(int id)
    {
        return await context.Users
                   .FirstOrDefaultAsync(u => u.Id == id) ??
               throw new NotFoundException();
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
}