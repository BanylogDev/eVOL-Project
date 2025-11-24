using DnsClient.Internal;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Data;
using eVOL.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        UserId = 11,
                        Name = "admin",
                        Email = "admin@evol.com",
                        Password = _passwordHasher.HashPassword("Admin123!"),
                        Role = "Admin",
                        CreatedAt = DateTime.UtcNow,
                    };

                    var sampleUser = new User
                    {
                        UserId = 12,
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
                        Id = 1,
                        Name = "General",
                        OwnerId = 1,
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
                        Id = 1,
                        Name = "General",
                        Category = "Test",
                        ClaimedStatus = false,
                        CreatedAt = DateTime.UtcNow,
                        OpenedById = 2,
                        ClaimedById = 1,
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
