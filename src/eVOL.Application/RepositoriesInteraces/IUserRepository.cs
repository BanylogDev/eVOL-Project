using eVOL.Application.DTOs.Responses.UserResponses.InfrastructureLayer;
using eVOL.Domain.Entities;

namespace eVOL.Application.RepositoriesInteraces
{
    public interface IUserRepository
    {
        Task<UserLoginFields?> GetUserLoginFields(string email, CancellationToken ct);
        Task<UserFields?> GetUserById(Guid id, CancellationToken ct);
        Task<UserTokenFields?> GetUserTokenFields(Guid id, CancellationToken ct);
        Task<ChatGroupUser?> GetUserForChatGroup(Guid id, CancellationToken ct);
        Task<User?> GetUserIdByEmail(string email, CancellationToken ct);
        Task<bool> CheckUserExistance(Guid id, CancellationToken ct);
        Task<bool> UpdateUserName(Guid id, string name, byte[] rowVersion, CancellationToken ct);
        Task<bool> UpdateUserEmail(Guid id, string email, byte[] rowVersion, CancellationToken ct);
        Task<bool> DeleteUser(Guid id, byte[] rowVersion, CancellationToken ct);
    }
}
