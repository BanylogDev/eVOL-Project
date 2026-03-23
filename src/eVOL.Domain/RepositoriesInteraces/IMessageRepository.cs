using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IMessageRepository
    {
        Task<ChatMessage?> GetChatMessageById(Guid id);
        Task<ChatMessage?> GetChatMessageBySenderId(Guid id);
        Task<ChatMessage?> GetChatMessageByReceiverId(Guid id);
        Task<ChatMessage?> AddChatMessageToDb(ChatMessage chatMessage);
        Task<ChatMessage?> DeleteChatMessageFromDb(ChatMessage chatMessage);
    }
}
