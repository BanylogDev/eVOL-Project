using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Domain.Enums;
using eVOL.Infrastructure.Persistence.Databases;
using Microsoft.EntityFrameworkCore;

namespace eVOL.Infrastructure.Repositories.AdminRepo
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserFields?> GetUserInfoAsync(Guid id, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new UserFields
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Address = u.Address,
                    Money = u.Money,
                    ChatGroups = u.ChatGroups,
                    SupportTickets = u.SupportTickets,
                    ClaimedTickets = u.ClaimedTickets,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync(ct);

        }

        public async Task<bool> DeleteUser(Guid id, CancellationToken ct)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == id)
                .ExecuteDeleteAsync(ct);

            return rowsAffected == 1;
        }

        public async Task<bool> BanUser(Guid userId, Guid adminId, DateTime bannedUntil, string reason, CancellationToken ct)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == userId && u.Ban.IsBanned == false)
                .ExecuteUpdateAsync(u => u
                .SetProperty(user => user.Ban.IsBanned, true)
                .SetProperty(user => user.Ban.BannedBy, adminId)
                .SetProperty(user => user.Ban.BannedUntil, bannedUntil)
                .SetProperty(user => user.Ban.Reason, reason)
                .SetProperty(user => user.Status, UserStatus.Banned),
                ct);

            return rowsAffected == 1;
        }

        public async Task<bool> UnBanUser(Guid userId, CancellationToken ct)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == userId && u.Ban.IsBanned == true)
                .ExecuteUpdateAsync(u => u
                .SetProperty(user => user.Ban.IsBanned, false)
                .SetProperty(user => user.Ban.BannedBy, Guid.Empty)
                .SetProperty(user => user.Ban.BannedUntil, DateTime.UtcNow)
                .SetProperty(user => user.Ban.Reason, string.Empty)
                .SetProperty(user => user.Status, UserStatus.Active),
                ct);

            return rowsAffected == 1;
        }
    }
}
