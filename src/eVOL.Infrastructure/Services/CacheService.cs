using eVOL.Application.ServicesInterfaces;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace eVOL.Infrastructure.Services;

public sealed class CacheService : ICacheService
{
    private readonly IDatabase _database;

    public CacheService(IConnectionMultiplexer multiplexer)
    {
        _database = multiplexer.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(
        string key,
        JsonTypeInfo<T> jsonTypeInfo,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        RedisValue value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        byte[] bytes = value!;

        return JsonSerializer.Deserialize(
            bytes,
            jsonTypeInfo);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        JsonTypeInfo<T> jsonTypeInfo,
        TimeSpan expiration,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(
            value,
            jsonTypeInfo);

        await _database.StringSetAsync(
            key,
            bytes,
            expiration);
    }

    public async Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _database.KeyDeleteAsync(key);
    }
}