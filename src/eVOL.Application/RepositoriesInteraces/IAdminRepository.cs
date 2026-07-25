using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;

namespace eVOL.Application.RepositoriesInteraces
{
    public interface IAdminRepository
    {
        Task<UserFields?> GetUserInfoAsync(Guid id, CancellationToken ct);
        Task<bool> DeleteUser(Guid id, CancellationToken ct);
        Task<bool> BanUser(Guid userId, Guid adminId, DateTime bannedUntil, string reason, CancellationToken ct);
        Task<bool> UnBanUser(Guid userId, CancellationToken ct);
    }
}
