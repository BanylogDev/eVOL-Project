using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace eVOL.Infrastructure.Repositories
{
    public class ChatGroupRepository : IChatGroupRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ChatGroupRepository> _logger;

        public ChatGroupRepository (ApplicationDbContext context, ICacheService cacheService, ILogger<ChatGroupRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<ChatGroup> CreateChatGroup(ChatGroup chatGroup)
        {
            await _context.ChatGroups.AddAsync(chatGroup);

            return chatGroup;
        }

        public ChatGroup? DeleteChatGroup(ChatGroup chatGroup)
        {

            _context.ChatGroups.Remove(chatGroup);

            return chatGroup;
        }

        public async Task<ChatGroup?> GetChatGroupById(Guid chatGroupId)
        {

            var cacheKey = $"chatGroup:{chatGroupId}";

            var cache = await _cacheService.GetAsync(cacheKey);

            if (cache != null)
            {
                _logger.LogInformation("");
                return JsonSerializer.Deserialize<ChatGroup>(cache);
            }

            _logger.LogInformation("");

            var chatGroup = await _context.ChatGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == chatGroupId);

            if (chatGroup != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(chatGroup),
                    TimeSpan.FromMinutes(10));
            }

            return chatGroup;
        }

        public async Task<ChatGroup?> GetChatGroupByName(string chatGroupName)
        {
            var cacheKey = $"chatGroup:{chatGroupName}";

            var cache = await _cacheService.GetAsync(cacheKey);

            if (cache != null)
            {
                _logger.LogInformation("");
                return JsonSerializer.Deserialize<ChatGroup>(cache);
            }

            _logger.LogInformation("");

            var chatGroup = await _context.ChatGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name == chatGroupName);

            if (chatGroup != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(chatGroup),
                    TimeSpan.FromMinutes(10));
            }

            return chatGroup;
        }

    }
}
