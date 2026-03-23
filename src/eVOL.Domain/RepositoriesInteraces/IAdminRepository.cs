using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IAdminRepository
    {
        Task<User?> GetUserInfoAsync(Guid id);
    }
}
