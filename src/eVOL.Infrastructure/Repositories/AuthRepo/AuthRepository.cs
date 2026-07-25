using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Persistence.Databases;
using Microsoft.EntityFrameworkCore;

namespace eVOL.Infrastructure.Repositories.AuthRepo
{
    public class AuthRepository : IAuthRepository
    {

        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(User newUser, CancellationToken ct)
        {
            return await _context.Users.AddAsync(newUser, ct) != null;
        }

        public async Task<UserPasswordField?> GetUserPasswordById(Guid id, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new UserPasswordField
                {
                    Password = u.Password,
                    RowVersion = u.RowVersion
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> UpdateUserPassword(Guid id, string password, byte[] rowVersion, CancellationToken ct)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == id &&
                u.RowVersion == rowVersion)
                .ExecuteUpdateAsync(u => u
                .SetProperty(user => user.Password, password), ct);

            return rowsAffected == 1;
        }

        public async Task<bool> UpdateRefreshToken(Guid id, string refreshToken, DateTime expiry, byte[] rowVersion, CancellationToken ct)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == id &&
                u.RowVersion == rowVersion)
                .ExecuteUpdateAsync(u => u
                .SetProperty(user => user.RefreshToken, refreshToken)
                .SetProperty(user => user.RefreshTokenExpiryTime, expiry));

            return rowsAffected == 1;
        }

    }
}
