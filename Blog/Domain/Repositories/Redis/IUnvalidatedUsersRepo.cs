namespace Blog.Domain.Repositories.Users;

public interface IUnvalidatedUsersRepo
{
    public Task AddValidationCodeAsync(string validationCode, string username);
    public Task<string> ValidateUserAsync(string validationCode);
    public Task RemoveExpiredValidationCodesAsync(List<string> expiredUsernames);
}