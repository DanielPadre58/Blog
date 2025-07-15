using Blog.Domain.Entities;

namespace Blog.Persistence.Repositories;

public interface IUserRepo
{
    public Task Create(User user);
    public Task<User> GetById(int id);
}