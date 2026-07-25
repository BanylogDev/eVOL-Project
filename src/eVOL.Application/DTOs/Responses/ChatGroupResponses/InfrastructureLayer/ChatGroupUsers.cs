using eVOL.Domain.Entities;

namespace eVOL.Application.DTOs.Responses.ChatGroupResponses.InfrastructureLayer
{
    public class ChatGroupUsers
    {
        public int TotalUsers { get; set; }
        public Guid OwnerId { get; set; }
        public ICollection<ChatGroupUser> Users { get; set; } = new List<ChatGroupUser>();
    }
}
