namespace eVOL.Domain.Entities
{
    public class SupportTicket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Text { get; set; }
        public int OpenedById { get; set; }
        public User OpenedBy { get; set; }

        public int ClaimedById { get; set; }
        public User ClaimedBy { get; set; }

        public bool ClaimedStatus { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<User> SupportTicketUsers { get; set; } = new List<User>();
    }
}
