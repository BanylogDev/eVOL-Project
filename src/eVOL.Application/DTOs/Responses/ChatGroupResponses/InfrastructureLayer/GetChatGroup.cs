using eVOL.Domain.Entities;

namespace eVOL.Application.DTOs.Responses.ChatGroupResponses.InfrastructureLayer
{
    public class GetChatGroup
    {
        public string Name { get; set; } = string.Empty;
        public int TotalUsers { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<ChatGroupUser> Users { get; set; } = new List<ChatGroupUser>();
    }
}
