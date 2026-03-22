using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IMessageRepository
    {
        Task<ChatMessage?> GetChatMessageById(int id);
        Task<ChatMessage?> GetChatMessageBySenderId(int id);
        Task<ChatMessage?> GetChatMessageByReceiverId(int id);
        Task<ChatMessage?> AddChatMessageToDb(ChatMessage chatMessage);
        Task<ChatMessage?> DeleteChatMessageFromDb(ChatMessage chatMessage);
    }
}
