using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.Domain.Repositories.Users;

public interface IUserRepo
{
    public Task<User> Create(User user);
    public Task Delete(int id);
    public Task<User> EditById(int id, UserUpdateDto updatedUser);
    public Task<User> GetById(int id);
    public Task<bool> UsernameExists(string username);
    public Task<List<User>> GetByUsernameUncapitalized(string username);
}