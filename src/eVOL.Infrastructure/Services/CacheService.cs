using eVOL.Application.ServicesInterfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVOL.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            Expiration redisExpiry = expiry.HasValue
                ? (Expiration)expiry.Value
                : Expiration.Persist;

            await _db.StringSetAsync(key, value, redisExpiry);
        }

        public async Task<string?> GetAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }
    }
}
