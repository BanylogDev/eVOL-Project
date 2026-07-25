using eVOL.Application.DTOs.Responses.ChatGroupResponses.InfrastructureLayer;
using eVOL.Application.RepositoriesInteraces;
using eVOL.Application.ServicesInterfaces;
using eVOL.Domain.Entities;
using eVOL.Infrastructure.Persistence.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eVOL.Infrastructure.Repositories
{
    public class ChatGroupRepository : IChatGroupRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ChatGroupRepository> _logger;

        public ChatGroupRepository(ApplicationDbContext context, ICacheService cacheService, ILogger<ChatGroupRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> CreateChatGroup(ChatGroup chatGroup, CancellationToken ct)
        {
            return await _context.ChatGroups.AddAsync(chatGroup, ct) != null;
        }

        public async Task<bool> DeleteChatGroup(Guid chatGroupId, Guid userId, CancellationToken ct)
        {

            return await _context.ChatGroups
                .Where(c => c.Id == chatGroupId && c.OwnerId == userId)
                .ExecuteDeleteAsync(ct) > 0;
        }

        public async Task<bool> DeleteChatGroupByName(string chatGroupName, Guid userId, CancellationToken ct)
        {

            return await _context.ChatGroups
                .Where(c => c.Name == chatGroupName && c.OwnerId == userId)
                .ExecuteDeleteAsync(ct) > 0;
        }

        public async Task<GetChatGroup?> GetChatGroupById(Guid chatGroupId, CancellationToken ct)
        {
            return await _context.ChatGroups
                .AsNoTracking()
                .Where(c => c.Id == chatGroupId)
                .Select(c => new GetChatGroup
                {
                    Name = c.Name,
                    OwnerId = c.OwnerId,
                    TotalUsers = c.TotalUsers,
                    Users = c.GroupUsers,
                    Messages = c.Messages,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ChatGroupUsers?> GetChatGroupUsersByName(string chatGroupName, CancellationToken ct)
        {
            return await _context.ChatGroups
                .AsNoTracking()
                .Where(c => c.Name == chatGroupName)
                .Select(c => new ChatGroupUsers
                {
                    Users = c.GroupUsers,
                    TotalUsers = c.TotalUsers,
                    OwnerId = c.OwnerId
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ChatGroupId?> GetChatGroupIdByName(string chatGroupName, CancellationToken ct)
        {
            return await _context.ChatGroups
                .AsNoTracking()
                .Where(c => c.Name == chatGroupName)
                .Select(c => new ChatGroupId
                {
                    Id = c.Id
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> TransferChatGroupOwnership(Guid currentOwnerId, Guid newOwnerId, Guid chatGroupId, CancellationToken ct)
        {
            return await _context.ChatGroups
                .Where(c => c.Id == chatGroupId && c.OwnerId == currentOwnerId && c.OwnerId == newOwnerId)
                .ExecuteUpdateAsync(c => c.SetProperty(chatGroup => chatGroup.OwnerId, newOwnerId), ct) > 0;
        }

    }
}
