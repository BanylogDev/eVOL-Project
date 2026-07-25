using eVOL.Domain.Entities;

namespace eVOL.Application.RepositoriesInteraces
{
    public interface IMessageRepository
    {
        Task<ChatMessage?> GetChatMessageById(Guid id, CancellationToken ct);
        Task<ChatMessage?> GetChatMessageBySenderId(Guid id, CancellationToken ct);
        Task<ChatMessage?> GetChatMessageByReceiverId(Guid id, CancellationToken ct);
        Task<bool> AddChatMessageToDb(ChatMessage chatMessage, CancellationToken ct);
        Task<bool> DeleteChatMessageFromDb(Guid id, CancellationToken ct);
    }
}
