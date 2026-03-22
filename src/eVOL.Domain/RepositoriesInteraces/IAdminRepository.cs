using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IAdminRepository
    {
        Task<User?> GetUserInfoAsync(int id);
    }
}
