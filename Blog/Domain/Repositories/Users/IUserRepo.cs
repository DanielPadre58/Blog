using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Users;

public interface IUserRepo
{
    public Task<User> Create(User user);
    public Task Delete(string username);
    public Task<User> Edit(string username, UserUpdateDto updatedUser);
    public Task<User> GetByUsername(string username);
    public Task<bool> UsernameExists(string username);
    public Task<List<User>> GetByUsernameUncapitalized(string username);
    public Task<User> VerifyUser(string username);
    public Task<List<string>> RemoveUnverifiedUsers();
}