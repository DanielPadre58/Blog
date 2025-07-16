using Blog.Application.Dtos.User;
using Blog.Domain.Entities;

namespace Blog.Persistence.Repositories.Users;

public interface IUserRepo
{
    public Task Create(User user);
    public Task Delete(int id);
    public Task EditById(int id, UserUpdateDto updatedUser);
    public Task<User> GetById(int id);
    public Task<bool> UsernameExists(string username);
    public Task<List<User>> GetByUsernameUncapitalized(string username);
    public Task AddLikeById(int userId, int postId);
}