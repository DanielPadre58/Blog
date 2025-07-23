using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Users;

public interface IUserRepo
{
    public Task<User> CreateAsync(User user);
    public Task DeleteAsync(string username);
    public Task<User> EditAsync(string username, UserUpdateDto updatedUser);
    public Task<User> GetByUsernameAsync(string username);
    public Task<bool> UsernameExistsAsync(string username);
    public Task<bool> EmailExistsAsync(string email);
    public Task<User> VerifyUserAsync(string username);
    public Task<List<string>> RemoveUnverifiedUsersAsync();
}