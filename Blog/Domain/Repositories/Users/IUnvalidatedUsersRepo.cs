namespace Blog.Domain.Repositories.Users;

public interface IUnvalidatedUsersRepo
{
    public Task AddValidationCode(string validationCode, string username);
    public Task<string> ValidateUser(string validationCode);
}