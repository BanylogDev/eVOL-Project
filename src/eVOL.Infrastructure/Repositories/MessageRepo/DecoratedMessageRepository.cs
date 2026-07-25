using DnsClient.Internal;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;

namespace eVOL.Infrastructure.Repositories.MessageRepo
{
    public class DecoratedMessageRepository : IMessageRepository
    {

        private readonly ILogger<DecoratedMessageRepository> _logger;
        private readonly MessageRepository _messageRepository;
        private readonly ICacheService _cacheService;

        public DecoratedMessageRepository(ILogger<DecoratedMessageRepository> logger, MessageRepository messageRepository, ICacheService cacheService)
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _cacheService = cacheService;
        }

        public async Task<bool> AddChatMessageToDb(ChatMessage chatMessage, CancellationToken ct)
        {
            _logger.LogInformation("Adding chat message to database: {ChatMessage} from DecoratedMessageRepository", chatMessage);

            return await _messageRepository.AddChatMessageToDb(chatMessage, ct);
        }

        public async Task<bool> DeleteChatMessageFromDb(Guid id, CancellationToken ct)
        {

            var cacheKey = $"chatMessage:{id}";

            _logger.LogInformation("Deleting chat message from database with id: {MessageId} from DecoratedMessageRepository", id);

            var check = await _messageRepository.DeleteChatMessageFromDb(id, ct);

            if (check) await _cacheService.RemoveAsync(cacheKey);

            return check;
        }

        public async Task<ChatMessage?> GetChatMessageById(Guid id, CancellationToken ct)
        {
            var cacheKey = $"chatMessage:{id}";

            var cache = await _cacheService.GetAsync(cacheKey, CacheJsonContext.Default.ChatMessage, ct);

            if (cache != null)
            {
                _logger.LogInformation("Cache hit in message repository by GetChatMessageById method");
                return cache;
            }

            _logger.LogInformation("Cache miss in message repository by GetChatMessageById method");

            var chatMessage = await _messageRepository.GetChatMessageById(id, ct);

            if (chatMessage != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    chatMessage,
                    CacheJsonContext.Default.ChatMessage,
                    TimeSpan.FromMinutes(10),
                    ct);
            }

            return chatMessage;
        }

        public async Task<ChatMessage?> GetChatMessageBySenderId(Guid id, CancellationToken ct)
        {
            var cacheKey = $"chatMessage:{id}";

            var cache = await _cacheService.GetAsync(cacheKey, CacheJsonContext.Default.ChatMessage, ct);

            if (cache != null)
            {
                _logger.LogInformation("Cache hit in message repository by GetChatMessageBySenderId method");
                return cache;
            }

            _logger.LogInformation("Cache miss in message repository by GetChatMessageBySenderId method");

            var chatMessage = await _messageRepository.GetChatMessageBySenderId(id, ct);

            if (chatMessage != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    chatMessage,
                    CacheJsonContext.Default.ChatMessage,
                    TimeSpan.FromMinutes(10),
                    ct);
            }

            return chatMessage;
        }

        public async Task<ChatMessage?> GetChatMessageByReceiverId(Guid id, CancellationToken ct)
        {
            var cacheKey = $"chatMessage:{id}";

            var cache = await _cacheService.GetAsync(cacheKey, CacheJsonContext.Default.ChatMessage, ct);

            if (cache != null)
            {
                _logger.LogInformation("Cache hit in message repository by GetChatMessageByReceiverId method");
                return cache;
            }

            _logger.LogInformation("Cache miss in message repository by GetChatMessageByReceiverId method");

            var chatMessage = await _messageRepository.GetChatMessageByReceiverId(id, ct);

            if (chatMessage != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    chatMessage,
                    CacheJsonContext.Default.ChatMessage,
                    TimeSpan.FromMinutes(10),
                    ct);
            }

            return chatMessage;
        }
    }
}
