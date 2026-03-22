using eVOL.Domain.Entities;

namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IAuthRepository
    {
        Task<User?> Register(User newUser);
    }
}
