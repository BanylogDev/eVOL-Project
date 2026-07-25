using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using eVOL.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;

namespace eVOL.Infrastructure.Repositories.AdminRepo
{
    public class DecoratedAdminRepository : IAdminRepository
    {

        private readonly ICacheService _cacheService;
        private readonly ILogger<DecoratedAdminRepository> _logger;
        private readonly AdminRepository _adminRepository;

        public DecoratedAdminRepository(ICacheService cacheService, ILogger<DecoratedAdminRepository> logger, AdminRepository adminRepository)
        {
            _cacheService = cacheService;
            _logger = logger;
            _adminRepository = adminRepository;
        }

        public async Task<UserFields?> GetUserInfoAsync(Guid id, CancellationToken ct)
        {
            var cacheKey = $"user:{id}";

            var cache = await _cacheService.GetAsync(cacheKey, CacheJsonContext.Default.UserFields, ct);

            if (cache != null)
            {
                _logger.LogInformation("Cache hit in user repository by GetUserInfoAsync method");
                return cache;
            }

            _logger.LogInformation("Cache miss in admin repository by GetUserInfoAsync method");

            var user = await _adminRepository.GetUserInfoAsync(id, ct);

            if (user != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    user,
                    CacheJsonContext.Default.UserFields,
                    TimeSpan.FromMinutes(10),
                    ct);
            }

            return user;
        }

        public async Task<bool> DeleteUser(Guid id, CancellationToken ct)
        {
            var cacheKey = $"user:{id}";

            _logger.LogInformation("Deleting user from in DecoratedUserRepository");

            var result = await _adminRepository.DeleteUser(id, ct);

            if (result) await _cacheService.RemoveAsync(cacheKey);

            return result;
        }

        public async Task<bool> BanUser(Guid userId, Guid adminId, DateTime bannedUntil, string reason, CancellationToken ct)
        {
            _logger.LogInformation("Banning user from in DecoratedUserRepository");

            return await _adminRepository.BanUser(userId, adminId, bannedUntil, reason, ct);
        }

        public async Task<bool> UnBanUser(Guid userId, CancellationToken ct)
        {
            _logger.LogInformation("UnBanning user from in DecoratedUserRepository");

            return await _adminRepository.UnBanUser(userId, ct);
        }
    }
}
