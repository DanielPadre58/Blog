using Blog.Shared.Exceptions;
using StackExchange.Redis;

namespace Blog.Domain.Repositories.Redis;

public class RedisRepo : IRedisRepo
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _redis;

    public RedisRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = _redis.GetDatabase();
    }

    public async Task SetRefreshTokenAsync(string username, string refreshToken)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(refreshToken))
            throw new InvalidFieldsException("Username and refresh token cannot be null or empty.");

        var expiration = TimeSpan.FromDays(30);
        await _db.StringSetAsync($"refresh_token:{username}", refreshToken, expiration);
    }

    public async Task<string> GetRefreshTokenAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new InvalidFieldsException("Username cannot be null or empty.");
        
        var refreshToken = await _db.StringGetAsync($"refresh_token:{username}");

        return refreshToken.IsNullOrEmpty ? 
            throw new NotFoundException($"No refresh token found for {username}")
            : refreshToken.ToString();
    }

    public async Task RemoveRefreshTokenAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
                throw new InvalidFieldsException("Username cannot be null or empty.");

        await _db.KeyDeleteAsync($"refresh_token:{username}");
    }
}