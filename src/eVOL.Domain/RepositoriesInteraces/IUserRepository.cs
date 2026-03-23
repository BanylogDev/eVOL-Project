using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByName(string email);
        Task<User?> GetUserByEmail(string email);
        Task<User?> UpdateUserCache(Guid id);
        void RemoveUser(User user);
    }
}
