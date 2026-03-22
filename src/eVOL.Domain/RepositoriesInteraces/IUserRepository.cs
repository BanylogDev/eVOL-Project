using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByName(string email);
        Task<User?> GetUserByEmail(string email);
        void RemoveUser(User user);
    }
}
