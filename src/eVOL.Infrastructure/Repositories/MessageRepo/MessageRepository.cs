using eVOL.Application.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Persistence.Databases;
using MongoDB.Driver;

namespace eVOL.Infrastructure.Repositories.MessageRepo
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MongoDbContext _context;

        private static FilterDefinition<ChatMessage> FilterByMessageId(Guid id) =>
            Builders<ChatMessage>.Filter.Eq(m => m.MessageId, id);

        private static FilterDefinition<ChatMessage> FilterBySenderId(Guid id) =>
            Builders<ChatMessage>.Filter.Eq(m => m.SenderId, id);

        private static FilterDefinition<ChatMessage> FilterByRecieverId(Guid id) =>
            Builders<ChatMessage>.Filter.Eq(m => m.ReceiverId, id);


        public MessageRepository(MongoDbContext context, ICacheService cacheService)
        {
            _context = context;
        }

        public async Task<ChatMessage?> GetChatMessageById(Guid id, CancellationToken ct)
        {
            return await _context.ChatMessages
                .Find(FilterByMessageId(id))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ChatMessage?> GetChatMessageBySenderId(Guid id, CancellationToken ct)
        {
            return await _context.ChatMessages
                .Find(FilterBySenderId(id))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ChatMessage?> GetChatMessageByReceiverId(Guid id, CancellationToken ct)
        {
            return await _context.ChatMessages
                .Find(FilterByRecieverId(id))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> AddChatMessageToDb(ChatMessage chatMessage, CancellationToken ct)
        {
            try
            {
                await _context.ChatMessages.InsertOneAsync(
                    chatMessage,
                    new InsertOneOptions(),
                    ct
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteChatMessageFromDb(Guid id, CancellationToken ct)
        {
            var check = await _context.ChatMessages.DeleteOneAsync(x => x.MessageId == id, ct);

            return check.IsAcknowledged && check.DeletedCount == 1;

        }
    }
}
