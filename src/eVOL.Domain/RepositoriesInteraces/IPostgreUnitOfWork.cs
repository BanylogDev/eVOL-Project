namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IPostgreUnitOfWork
    {
        IUserRepository Users { get; }
        IAuthRepository Auth { get; }
        IAdminRepository Admin { get; }
        IChatGroupRepository ChatGroup { get; }
        ISupportTicketRepository SupportTicket { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
