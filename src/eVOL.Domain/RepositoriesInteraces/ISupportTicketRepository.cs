using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface ISupportTicketRepository
    {
        Task<SupportTicket> CreateSupportTicket(SupportTicket supportTicket);
        SupportTicket? DeleteSupportTicket(SupportTicket supportTicket);
        Task<SupportTicket?> GetSupportTicketById(Guid id);
        Task<SupportTicket?> GetSupportTicketByName(string name);
    }
}
