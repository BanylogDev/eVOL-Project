using eVOL.Application.RepositoriesInteraces;
using eVOL.Application.RepositoriesInteraces.UnitsOfWork;
using eVOL.Infrastructure.Persistence.Databases;
using Microsoft.EntityFrameworkCore.Storage;

namespace eVOL.Infrastructure.Persistence.UnitsOfWork
{
    public class PostgreUnitOfWork : IPostgreUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public IUserRepository Users { get; }
        public IAuthRepository Auth { get; }
        public IAdminRepository Admin { get; }
        public IChatGroupRepository ChatGroup { get; }
        public ISupportTicketRepository SupportTicket { get; }

        public PostgreUnitOfWork(ApplicationDbContext dbContext,
            IUserRepository users,
            IAuthRepository auth,
            IAdminRepository admin,
            IChatGroupRepository chatGroup,
            ISupportTicketRepository supportTicket)
        {
            _dbContext = dbContext;
            Users = users;
            Auth = auth;
            Admin = admin;
            ChatGroup = chatGroup;
            SupportTicket = supportTicket;
        }

        public async Task BeginTransactionAsync() =>
            _transaction = await _dbContext.Database.BeginTransactionAsync();

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }
    }

}
