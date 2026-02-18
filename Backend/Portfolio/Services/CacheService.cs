using Portfolio.IServices;
using StackExchange.Redis;

public class CacheService: ICacheService
{
    private readonly IDatabase _redis;

    public CacheService(IConnectionMultiplexer mux)
    {
        _redis = mux.GetDatabase();
    }

    public async Task SaveTokenAsync(
        string key,
        string value,
        int expiresInSeconds)
    {
        await _redis.StringSetAsync(key, value, TimeSpan.FromSeconds(expiresInSeconds - 60));
    }

    public async Task<string?> GetTokenAsync(string key)
    {
        return await _redis.StringGetAsync(key);
    }

    public async Task RemoveTokenAsync(string key)
    {
        await _redis.KeyDeleteAsync(key);
    }
}
