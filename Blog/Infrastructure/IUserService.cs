using Blog.Domain.Entities;

namespace Blog.Infrastructure;

public interface IUserService
{
    public Task<User>? Create(User user);
}