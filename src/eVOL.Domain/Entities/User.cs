using eVOL.Domain.Enums;
using eVOL.Domain.ValueObjects;

namespace eVOL.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Address? Address { get; set; }
        public string Role { get; set; } = "User";
        public Money? Money { get; set; }
        public Ban? Ban { get; set; }
        public UserStatus Status { get; set; }
        public DateTime LastActiveAt { get; set; }
        public DateTime InactivatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ChatGroup> ChatGroups { get; set; } = new List<ChatGroup>();
        public ICollection<SupportTicket> ClaimedTickets { get; set; } = new List<SupportTicket>();
        public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
        public ICollection<SupportTicket> OpenedTickets { get; set; } = new List<SupportTicket>();


        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public byte[]? RowVersion { get; init; }
    }
}
