using Blog.Domain.Entities;

namespace Blog.Persistence.Repositories.Users;

public interface IUserRepo
{
    public Task Create(User user);
    public Task Delete(int id);
    public Task<User> GetById(int id);
    public Task<List<User>> GetByUsername(string username);
    public Task<List<User>> GetByUsernameUncapitalized(string username);
}