namespace eVOL.Application.RepositoriesInteraces.UnitsOfWork
{
    public interface IMongoUnitOfWork
    {
        IMessageRepository Message { get; }

        void BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
