using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Infrastructure.Data;

namespace eVOL.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> Register(User newUser)
        {
            await _context.Users.AddAsync(newUser);

            return newUser;
        }

    }
}
