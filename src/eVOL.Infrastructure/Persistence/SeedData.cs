using DnsClient.Internal;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace eVOL.Infrastructure.Persistence
{
    public class SeedData
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<SeedData> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public SeedData(ApplicationDbContext context, ILogger<SeedData> logger, IPasswordHasher passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task InitializeAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();

            _logger.LogInformation("Started Transaction in SeedData Initialization");

            try
            {

                if (!_context.Users.Any())
                {
                    var adminUser = new User
                    {
                        UserId = Guid.Parse("92004000-0000-0000-0000-000000000001"),
                        Name = "admin",
                        Email = "admin@evol.com",
                        Password = _passwordHasher.HashPassword("Admin123!"),
                        Role = "Admin",
                        CreatedAt = DateTime.UtcNow,
                    };

                    var sampleUser = new User
                    {
                        UserId = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                        Name = "Sample User",
                        Email = "sampleuser@evol.com",
                        Password = _passwordHasher.HashPassword("sampleUser123!"),
                        Role = "User",
                        CreatedAt = DateTime.UtcNow,
                    };

                    await _context.Users.AddAsync(adminUser);
                    await _context.Users.AddAsync(sampleUser);
                }

                _logger.LogInformation("Seeded Database with Admin & Sample User");

                if (!_context.ChatGroups.Any())
                {
                    await _context.ChatGroups.AddAsync(new ChatGroup
                    {
                        Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                        Name = "General",
                        OwnerId = Guid.Parse("92004000-0000-0000-0000-000000000001"),
                        CreatedAt = DateTime.UtcNow,
                        TotalUsers = 1,
                        GroupUsers = new List<User>()
                    });
                }

                _logger.LogInformation("Seeded Database with ChatGroup");

                if (!_context.SupportTickets.Any())
                {
                    await _context.SupportTickets.AddAsync(new SupportTicket
                    {
                        Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                        Name = "General",
                        Category = "Test",
                        ClaimedStatus = false,
                        CreatedAt = DateTime.UtcNow,
                        OpenedById = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                        ClaimedById = Guid.Parse("92004000-0000-0000-0000-000000000001"),
                        Text = "Test",
                        SupportTicketUsers = new List<User>()
                    });
                }

                _logger.LogInformation("Seeded Database with SupportTicket");

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Seeded Database Completed Successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Something wen't wrong during database seeding!");
                throw;
            }
        }
    }
}
