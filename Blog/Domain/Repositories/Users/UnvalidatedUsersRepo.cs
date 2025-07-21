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

    public async Task AddValidationCode(string validationCode, string username)
    {
        var result = await _db.StringSetAsync(validationCode, username, TimeSpan.FromMinutes(10));

        if (!result)
            throw new Exception("Validation code could not be set.");
    }

    public async Task<string> ValidateUser(string validationCode)
    {
        if (!await _db.KeyExistsAsync(validationCode))
            return null;

        var username = _db.StringGetAsync(validationCode) ??
                       throw new NotFoundException("Validation code not found or expired.");
        
        await _db.KeyDeleteAsync(validationCode);
        
        return username.ToString();;
    }
}