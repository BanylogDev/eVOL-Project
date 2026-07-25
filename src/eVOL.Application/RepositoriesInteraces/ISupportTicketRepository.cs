using eVOL.Application.DTOs.Responses.SupportTicketResponses.RepositoryLayer;
using eVOL.Domain.Entities;

namespace eVOL.Application.RepositoriesInteraces
{
    public interface ISupportTicketRepository
    {
        Task<bool> CreateSupportTicket(SupportTicket supportTicket, CancellationToken ct);
        Task<bool> DeleteSupportTicket(Guid id, CancellationToken ct);
        Task<SupportTicketFields?> GetSupportTicketById(Guid id, CancellationToken ct);
        Task<SupportTicketMessageIds?> GetSupportTicketByName(string name, CancellationToken ct);
        Task<bool> ClaimSupportTicket(Guid SupportTicketId, Guid ClaimerId, CancellationToken ct);
        Task<bool> UnClaimSupportTicket(Guid SupportTicketId, CancellationToken ct);
    }
}
