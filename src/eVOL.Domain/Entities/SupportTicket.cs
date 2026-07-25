namespace eVOL.Domain.Entities
{
    public class SupportTicket
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public Guid OpenedById { get; set; }
        public User? OpenedBy { get; set; }

        public Guid ClaimedById { get; set; }
        public User? ClaimedBy { get; set; }

        public bool ClaimedStatus { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<User> SupportTicketUsers { get; set; } = new List<User>();

        public byte[]? RowVersion { get; init; }
    }
}
