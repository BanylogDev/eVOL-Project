using eVOL.Application.DTOs.Responses.SupportTicketResponses.RepositoryLayer;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Persistence.Databases;
using Microsoft.EntityFrameworkCore;

namespace eVOL.Infrastructure.Repositories.SupportTicketRepo
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        private readonly ApplicationDbContext _context;

        public SupportTicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateSupportTicket(SupportTicket supportTicket, CancellationToken ct)
        {

            return await _context.SupportTickets.AddAsync(supportTicket, ct) != null;

        }

        public async Task<bool> DeleteSupportTicket(Guid id, CancellationToken ct)
        {

            var rowsAffected = await _context.SupportTickets
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(ct);

            return rowsAffected > 0;

        }

        public async Task<SupportTicketFields?> GetSupportTicketById(Guid id, CancellationToken ct)
        {
            return await _context.SupportTickets
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new SupportTicketFields
                {
                    Name = s.Name,
                    Category = s.Category,
                    OpenedById = s.OpenedById,
                    ClaimedById = s.ClaimedById,
                    ClaimedStatus = s.ClaimedStatus,
                    CreatedAt = s.CreatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<SupportTicketMessageIds?> GetSupportTicketByName(string name, CancellationToken ct)
        {
            return await _context.SupportTickets
               .AsNoTracking()
               .Where(s => s.Name == name)
               .Select(s => new SupportTicketMessageIds
               {
                   SupportTicketId = s.Id,
                   UserId = s.User.UserId
               })
               .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> ClaimSupportTicket(Guid SupportTicketId, Guid ClaimerId, CancellationToken ct)
        {
            var rowsAffected = await _context.SupportTickets
                .Where(s => s.Id == SupportTicketId && s.ClaimedStatus == false)
                .ExecuteUpdateAsync(s => s
                .SetProperty(ticket => ticket.ClaimedById, ClaimerId)
                .SetProperty(ticket => ticket.ClaimedStatus, true));

            return rowsAffected == 1;
        }

        public async Task<bool> UnClaimSupportTicket(Guid SupportTicketId, CancellationToken ct)
        {
            var rowsAffected = await _context.SupportTickets
                .Where(s => s.Id == SupportTicketId && s.ClaimedStatus == true)
                .ExecuteUpdateAsync(s => s
                .SetProperty(ticket => ticket.ClaimedById, Guid.Empty)
                .SetProperty(ticket => ticket.ClaimedStatus, false));

            return rowsAffected == 1;
        }
    }
}
