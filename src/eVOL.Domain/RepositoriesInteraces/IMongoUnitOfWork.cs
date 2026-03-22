namespace eVOL.Domain.RepositoriesInteraces
{
    public interface IMongoUnitOfWork
    {
        IMessageRepository Message { get; }

        void BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
