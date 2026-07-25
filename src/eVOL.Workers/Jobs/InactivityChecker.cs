using eVOL.Domain.Entities;
using eVOL.Domain.Enums;
using eVOL.Infrastructure.Persistence.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eVOL.Workers.Jobs
{
    public class InactivityChecker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<InactivityChecker> _logger;
        private const int BatchSize = 1000;
        public InactivityChecker(IServiceScopeFactory scopeFactory,
            ILogger<InactivityChecker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromDays(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var mainContext =
                        scope.ServiceProvider
                             .GetRequiredService<ApplicationDbContext>();

                    var archivedContext =
                        scope.ServiceProvider
                             .GetRequiredService<ArchivedDbContext>();


                    var now = DateTime.UtcNow;

                    var inactivityCutoff = now.AddMonths(-6);

                    var archivedCutoff = now.AddYears(-2);

                    // Inactive

                    var inactivityUpdatedCount = await mainContext.Users
                        .Where(u => u.Status == UserStatus.Active &&
                                    u.LastActiveAt < inactivityCutoff)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(u => u.Status, UserStatus.Inactive)
                            .SetProperty(u => u.InactivatedAt, DateTime.UtcNow),
                            cancellationToken: stoppingToken);

                    // Archived

                    int totalArchived = 0;

                    var (usersToArchive, lastId) = await ArchiveRead(mainContext, archivedCutoff, null, stoppingToken);

                    while (!stoppingToken.IsCancellationRequested && usersToArchive.Count > 0)
                    {
                        var archivedCount = usersToArchive.Count;

                        var userIds = usersToArchive
                            .Select(u => u.UserId)
                            .ToList();

                        foreach (var user in usersToArchive)
                        {
                            user.Status = UserStatus.Archived;
                            user.InactivatedAt = now;
                        }

                        await archivedContext.Users.AddRangeAsync(usersToArchive, stoppingToken);
                        await archivedContext.SaveChangesAsync(stoppingToken);

                        await mainContext.Users
                            .Where(u => userIds.Contains(u.UserId))
                            .ExecuteDeleteAsync(stoppingToken);

                        totalArchived += archivedCount;

                        (usersToArchive, lastId) = await ArchiveRead(mainContext, archivedCutoff, lastId, stoppingToken);

                    }

                    _logger.LogInformation(
                        "Inactivity check completed. {InactiveCount} users marked inactive. {TotalArchived} users archived.",
                        inactivityUpdatedCount,
                        totalArchived);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error running inactivity check.");
                }
            }
        }

        private async Task<(List<User> Users, Guid LastId)> ArchiveRead(ApplicationDbContext mainContext, DateTime archivedCutoff, Guid? lastId, CancellationToken stoppingToken)
        {

            var query = mainContext.Users
                .Where(u =>
                    u.Status == UserStatus.Inactive &&
                    u.LastActiveAt < archivedCutoff);

            if (lastId != null)
            {
                query = query.Where(u => u.UserId > lastId);
            }

            var users = await query
                .OrderBy(u => u.UserId)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);

            if (users.Count == 0)
                return (users, Guid.Empty);

            return (users, users[^1].UserId);

        }
    }
}
