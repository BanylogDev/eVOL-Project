using eVOL.Application.DTOs.Responses.ChatGroupResponses.InfrastructureLayer;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;

namespace eVOL.Infrastructure.Repositories.ChatGroupRepo
{
    public class DecoratedChatGroupRepository : IChatGroupRepository
    {

        private readonly ChatGroupRepository _chatGroupRepository;
        private readonly ILogger<DecoratedChatGroupRepository> _logger;
        private readonly ICacheService _cacheService;

        public DecoratedChatGroupRepository(ChatGroupRepository chatGroupRepository, ILogger<DecoratedChatGroupRepository> logger, ICacheService cacheService)
        {
            _chatGroupRepository = chatGroupRepository;
            _logger = logger;
            _cacheService = cacheService;
        }


        public async Task<bool> CreateChatGroup(ChatGroup chatGroup, CancellationToken ct)
        {
            return await _chatGroupRepository.CreateChatGroup(chatGroup, ct);
        }

        public async Task<bool> DeleteChatGroup(Guid chatGroupId, Guid userId, CancellationToken ct)
        {
            var result = await _chatGroupRepository.DeleteChatGroup(chatGroupId, userId, ct);

            var cacheKey = $"chatGroup:{chatGroupId}";
            var cacheKey2 = $"chatGroupUsers:{chatGroupId}";

            if (result)
            {
                await _cacheService.RemoveAsync(cacheKey);
                await _cacheService.RemoveAsync(cacheKey2);
            }

            return result;
        }

        public async Task<bool> DeleteChatGroupByName(string chatGroupName, Guid userId, CancellationToken ct)
        {
            return await _chatGroupRepository.DeleteChatGroupByName(chatGroupName, userId, ct);
        }

        public async Task<GetChatGroup?> GetChatGroupById(Guid chatGroupId, CancellationToken ct)
        {

            var cacheKey = $"chatGroup:{chatGroupId}";

            var cache = await _cacheService.GetAsync(cacheKey, CacheJsonContext.Default.GetChatGroup, ct);

            if (cache != null)
            {
                _logger.LogInformation("Cache hit in DecoratedChatGroupRepository by GetChatGroupById method");
                return cache;
            }

            _logger.LogInformation("Cache miss in DecoratedChatGroupRepository by GetChatGroupById method");

            var chatGroup = await _chatGroupRepository.GetChatGroupById(chatGroupId, ct);

            if (chatGroup != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    chatGroup,
                    CacheJsonContext.Default.GetChatGroup,
                    TimeSpan.FromMinutes(10),
                    ct);
            }

            return chatGroup;
        }

        public async Task<ChatGroupUsers?> GetChatGroupUsersByName(string chatGroupName, CancellationToken ct)
        {

            var cacheKey = $"chatGroupUsers:{chatGroupName}";

            var cache = await _cacheService.GetAsync(cacheKey, CacheJsonContext.Default.ChatGroupUsers);

            if (cache != null)
            {
                _logger.LogInformation("Cache miss in DecoratedChatGroupRepository by GetChatGroupByName method");
                return cache;
            }

            _logger.LogInformation("Cache miss in DecoratedChatGroupRepository by GetChatGroupByName method");

            var chatGroup = await _chatGroupRepository.GetChatGroupUsersByName(chatGroupName, ct);

            if (chatGroup != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    chatGroup,
                    CacheJsonContext.Default.ChatGroupUsers,
                    TimeSpan.FromMinutes(10),
                    ct);
            }

            return chatGroup;
        }

        public async Task<ChatGroupId?> GetChatGroupIdByName(string chatGroupName, CancellationToken ct)
        {
            return await _chatGroupRepository.GetChatGroupIdByName(chatGroupName, ct);
        }

        public async Task<bool> TransferChatGroupOwnership(Guid currentOwnerId, Guid newOwnerId, Guid chatGroupId, CancellationToken ct)
        {
            return await _chatGroupRepository.TransferChatGroupOwnership(currentOwnerId, newOwnerId, chatGroupId, ct);
        }
    }
}
