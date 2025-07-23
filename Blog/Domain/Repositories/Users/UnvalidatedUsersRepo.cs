using Blog.Shared.Exceptions;
using StackExchange.Redis;

namespace Blog.Domain.Repositories.Users;

public class UnvalidatedUsersRepo : IUnvalidatedUsersRepo
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _redis;

    public UnvalidatedUsersRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = _redis.GetDatabase();
    }

    public async Task AddValidationCodeAsync(string username, string code)
    {
        var ttl = TimeSpan.FromMinutes(10);

        var setUsername = await _db.StringSetAsync($"validation:{username}", code, ttl);
        var setCode = await _db.StringSetAsync($"code:{code}", username, ttl);

        if (!setUsername || !setCode)
            throw new Exception("Failed to store validation code.");
    }

    public async Task<string?> ValidateUserAsync(string code)
    {
        var username = await _db.StringGetAsync($"code:{code}");
        if (username.IsNullOrEmpty)
            return null;

        await _db.KeyDeleteAsync($"code:{code}");
        await _db.KeyDeleteAsync($"validation:{username}");

        return username.ToString();
    }

    public async Task RemoveExpiredValidationCodesAsync(List<string> expiredUsernames)
    {
        foreach (var username in expiredUsernames)
        {
            var code = await _db.StringGetAsync($"validation:{username}");
            await _db.KeyDeleteAsync($"code:{code.ToString()}");
            await _db.KeyDeleteAsync($"validation:{username}");
        }
    }
}