using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IChatGroupRepository
    {
        Task<ChatGroup> CreateChatGroup(ChatGroup chatGroup);
        ChatGroup? DeleteChatGroup(ChatGroup chatGroup);
        Task<ChatGroup?> GetChatGroupById(int chatGroupId);
        Task<ChatGroup?> GetChatGroupByName(string chatGroupName);
    }
}
