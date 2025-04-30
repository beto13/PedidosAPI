using Application.Common.Interfaces.Repositories.Persistence;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        ISqlCommandRepository<T> CommandRepository<T>() where T : class;
        ISqlQueryRepository<T> QueryRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task ExecuteTransactionAsync(Func<Task> operation);
    }
}
