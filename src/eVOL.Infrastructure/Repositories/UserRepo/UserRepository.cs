using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Persistence.Databases;
using Microsoft.EntityFrameworkCore;

namespace eVOL.Infrastructure.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private static readonly Func<ApplicationDbContext, string, IAsyncEnumerable<UserLoginFields>>
            GetUserLoginFieldsCompiled = EF.CompileAsyncQuery((ApplicationDbContext context, string email)
                => context.Users
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => new UserLoginFields
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                Password = u.Password,
                RowVersion = u.RowVersion
            }));

        private static readonly Func<ApplicationDbContext, Guid, IAsyncEnumerable<UserFields>>
            GetUserByIdCompiled = EF.CompileAsyncQuery((ApplicationDbContext context, Guid id)
                => context.Users
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
                Ban = u.Ban,
                CreatedAt = u.CreatedAt
            }));

        public UserRepository(ApplicationDbContext context) { _context = context; }

        public async Task<UserLoginFields?> GetUserLoginFields(string email, CancellationToken ct)
        {
            await foreach (var user in GetUserLoginFieldsCompiled(_context, email).WithCancellation(ct))
                return user;

            return null;
        }

        public async Task<UserTokenFields?> GetUserTokenFields(Guid id, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new UserTokenFields
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    RefreshToken = u.RefreshToken,
                    RefreshTokenExpiryTime = u.RefreshTokenExpiryTime
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<UserFields?> GetUserById(Guid id, CancellationToken ct)
        {
            await foreach (var user in GetUserByIdCompiled(_context, id).WithCancellation(ct))
                return user;

            return null;
        }

        public async Task<ChatGroupUser?> GetUserForChatGroup(Guid id, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new ChatGroupUser
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<User?> GetUserIdByEmail(string email, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => new User
                {
                    UserId = u.UserId,
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> CheckUserExistance(Guid id, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new User
                {
                    UserId = u.UserId,
                })
                .FirstOrDefaultAsync(ct) != null;
        }

        public async Task<bool> UpdateUserName(Guid id, string name, byte[] rowVersion, CancellationToken ct)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == id &&
                u.RowVersion == rowVersion)
                .ExecuteUpdateAsync(u => u
                .SetProperty(user => user.Name, name), ct);

            return rowsAffected == 1;
        }

        public async Task<bool> UpdateUserEmail(Guid id, string email, byte[] rowVersion, CancellationToken ct)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == id &&
                u.RowVersion == rowVersion)
                .ExecuteUpdateAsync(u => u
                .SetProperty(user => user.Email, email), ct);

            return rowsAffected == 1;
        }

        public async Task<bool> DeleteUser(Guid id, byte[] rowVersion, CancellationToken ct)
        {

            var rowsAffected = await _context.Users
                .Where(u => u.UserId == id &&
                u.RowVersion == rowVersion)
                .ExecuteDeleteAsync(ct);

            return rowsAffected == 1;

        }


    }
}
