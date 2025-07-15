using Blog.Persistence.Repositories;

namespace Blog.Infrastructure;

public class UserService(IUserRepo repository) : IUserService
{

}