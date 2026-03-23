using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Domain.RepositoriesInteraces;
using eVOL.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace eVOL.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AdminRepository> _logger;
        
        public AdminRepository(ApplicationDbContext context, ICacheService cacheService, ILogger<AdminRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<User?> GetUserInfoAsync(Guid id)
        {
            var cacheKey = $"users:{id}";

            var cache = await _cacheService.GetAsync(cacheKey);

            if (cache != null)
            {
                _logger.LogInformation("Cache hit in user repository by GetUserInfoAsync method");
                return JsonSerializer.Deserialize<User>(cache);
            }

            _logger.LogInformation("Cache miss in admin repository by GetUserInfoAsync method");

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user != null)
            {
                await _cacheService.SetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(user),
                    TimeSpan.FromMinutes(10));
            }

            return user;
        }
    }
}
