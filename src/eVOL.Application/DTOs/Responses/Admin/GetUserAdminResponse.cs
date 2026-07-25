using eVOL.Application.DTOs.Responses.UserResponses.ApplicationLayer;
using eVOL.Domain.Entities;
using eVOL.Domain.ValueObjects;

namespace eVOL.Application.DTOs.Responses.Admin
{
    public sealed class GetUserAdminResponse : BaseUserResponse
    {
        public Address? Address { get; set; }
        public Money? Money { get; set; }
        public ICollection<ChatGroup>? ChatGroups { get; set; }
        public ICollection<SupportTicket>? SupportTickets { get; set; }
        public ICollection<SupportTicket>? ClaimedTickets { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
