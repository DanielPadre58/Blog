namespace Blog.Domain.Repositories.Redis;

public interface IRedisRepo
{
    public Task SetRefreshTokenAsync(string username, string refreshToken);
    public Task<string> GetRefreshTokenAsync(string username);
    public Task RemoveRefreshTokenAsync(string username);
}