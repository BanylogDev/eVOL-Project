using eVOL.Application.DTOs.Responses.ChatGroupResponses.InfrastructureLayer;
using eVOL.Domain.Entities;

namespace eVOL.Application.RepositoriesInteraces
{
    public interface IChatGroupRepository
    {
        Task<bool> CreateChatGroup(ChatGroup chatGroup, CancellationToken ct);
        Task<bool> DeleteChatGroup(Guid chatGroupId, Guid userId, CancellationToken ct);
        Task<bool> DeleteChatGroupByName(string chatGroupName, Guid userId, CancellationToken ct);
        Task<GetChatGroup?> GetChatGroupById(Guid chatGroupId, CancellationToken ct);
        Task<ChatGroupUsers?> GetChatGroupUsersByName(string chatGroupName, CancellationToken ct);
        Task<ChatGroupId?> GetChatGroupIdByName(string chatGroupName, CancellationToken ct);
        Task<bool> TransferChatGroupOwnership(Guid currentOwnerId, Guid newOwnerId, Guid chatGroupId, CancellationToken ct);
    }
}
