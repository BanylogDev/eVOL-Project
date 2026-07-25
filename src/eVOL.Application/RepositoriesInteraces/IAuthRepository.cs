using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;
using eVOL.Domain.Entities;

namespace eVOL.Application.RepositoriesInteraces
{
    public interface IAuthRepository
    {
        Task<bool> Register(User newUser, CancellationToken ct);
        Task<UserPasswordField?> GetUserPasswordById(Guid id, CancellationToken ct);
        Task<bool> UpdateUserPassword(Guid id, string password, byte[] rowVersion, CancellationToken ct);
        Task<bool> UpdateRefreshToken(Guid id, string refreshToken, DateTime expiry, byte[] rowVersion, CancellationToken ct);
    }
}
