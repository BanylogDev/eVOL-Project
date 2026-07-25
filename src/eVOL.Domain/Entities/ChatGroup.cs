namespace eVOL.Domain.Entities
{
    public class ChatGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalUsers { get; set; }
        public Guid OwnerId { get; set; }
        public User? Owner { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<ChatGroupUser> GroupUsers { get; set; } = new List<ChatGroupUser>();

        public byte[]? RowVersion { get; init; }
    }
}
