namespace Blog.Persistence.Repositories.Users;

public interface IUserRepo
{
    public Task Create(Domain.Entities.User user);
    public Task Delete(int id);
    public Task<Domain.Entities.User> GetById(int id);
    public Task<List<Domain.Entities.User>> GetByUsername(string username);
    public Task<List<Domain.Entities.User>> GetByUsernameUncapitalized(string username);
}