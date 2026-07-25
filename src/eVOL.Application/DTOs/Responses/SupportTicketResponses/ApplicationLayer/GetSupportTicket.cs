using eVOL.Application.DTOs.Responses.Global;

namespace eVOL.Application.DTOs.Responses.SupportTicketResponses.ApplicationLayer
{
    public class GetSupportTicket : ResultResponse
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public Guid OpenedById { get; set; }

        public Guid ClaimedById { get; set; }

        public bool ClaimedStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
