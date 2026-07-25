using eVOL.Domain.Entities;
using eVOL.Domain.ValueObjects;

namespace eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer
{
    public class UserFields
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Address? Address { get; set; }
        public string Role { get; set; } = "User";
        public Money? Money { get; set; }
        public Ban? Ban { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ChatGroup> ChatGroups { get; set; } = new List<ChatGroup>();
        public ICollection<SupportTicket> ClaimedTickets { get; set; } = new List<SupportTicket>();
        public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

        public byte[]? RowVersion { get; init; }
    }
}
