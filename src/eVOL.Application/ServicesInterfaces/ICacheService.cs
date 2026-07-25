using System.Text.Json.Serialization.Metadata;

namespace eVOL.Application.ServicesInterfaces
{

    public interface ICacheService
    {
        Task<T?> GetAsync<T>(
            string key,
            JsonTypeInfo<T> jsonTypeInfo,
            CancellationToken cancellationToken = default);

        Task SetAsync<T>(
            string key,
            T value,
            JsonTypeInfo<T> jsonTypeInfo,
            TimeSpan expiration,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default);
    }
}
