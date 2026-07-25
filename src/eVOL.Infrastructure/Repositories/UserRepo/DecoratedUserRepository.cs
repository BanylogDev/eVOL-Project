using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;

namespace eVOL.Infrastructure.Repositories.UserRepo
{
    public class DecoratedUserRepository : IUserRepository
    {

        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<UserRepository> _logger;

        public DecoratedUserRepository(ICacheService cacheService, ILogger<UserRepository> logger, IUserRepository userRepository)
        {
            _cacheService = cacheService;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<UserFields?> GetUserById(Guid id, CancellationToken ct)
        {
            var cacheKey = $"user:{id}";

            var cached = await _cacheService.GetAsync(cacheKey, CacheJsonContext.Default.UserFields, ct);

            if (cached != null)
            {
                _logger.LogInformation("Cache hit in DecoratedUserRepository by GetUserById method");
                return cached;
            }

            _logger.LogInformation("Cache miss in DecoratedUserRepository by GetUserById method");

            var user = await _userRepository.GetUserById(id, ct);

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

        public async Task<ChatGroupUser?> GetUserForChatGroup(Guid id, CancellationToken ct)
        {
            return await _userRepository.GetUserForChatGroup(id, ct);
        }

        public async Task<User?> GetUserIdByEmail(string email, CancellationToken ct)
        {
            _logger.LogInformation("Retrieving user by email from in DecoratedUserRepository");

            return await _userRepository.GetUserIdByEmail(email, ct);
        }

        public async Task<bool> DeleteUser(Guid id, byte[] rowVersion, CancellationToken ct)
        {

            var cacheKey = $"user:{id}";

            _logger.LogInformation("Deleting user from in DecoratedUserRepository");

            var check = await _userRepository.DeleteUser(id, rowVersion, ct);

            if (check) await _cacheService.RemoveAsync(cacheKey);

            return check;
        }

        public async Task<UserLoginFields?> GetUserLoginFields(string email, CancellationToken ct)
        {
            _logger.LogInformation("Retrieving user login fields from in DecoratedUserRepository");

            return await _userRepository.GetUserLoginFields(email, ct);

        }

        public async Task<bool> CheckUserExistance(Guid id, CancellationToken ct)
        {
            _logger.LogInformation("Checking user existance from in DecoratedUserRepository");

            return await _userRepository.CheckUserExistance(id, ct);
        }

        public async Task<bool> UpdateUserName(Guid id, string name, byte[] rowVersion, CancellationToken ct)
        {
            var cacheKey = $"user:{id}";

            var result = await _userRepository.UpdateUserName(id, name, rowVersion, ct);

            if (result) await _cacheService.RemoveAsync(cacheKey);

            return result;
        }

        public async Task<bool> UpdateUserEmail(Guid id, string email, byte[] rowVersion, CancellationToken ct)
        {
            var cacheKey = $"user:{id}";

            var result = await _userRepository.UpdateUserEmail(id, email, rowVersion, ct);

            if (result) await _cacheService.RemoveAsync(cacheKey);

            return result;
        }

        public async Task<UserTokenFields?> GetUserTokenFields(Guid id, CancellationToken ct)
        {
            _logger.LogInformation("Retrieving user token fields from in DecoratedUserRepository");

            return await _userRepository.GetUserTokenFields(id, ct);
        }
    }
}
