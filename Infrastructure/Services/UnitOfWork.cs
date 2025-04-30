using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Repositories.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _commandRepositories = new();
        private readonly Dictionary<Type, object> _queryRepositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ISqlCommandRepository<T> CommandRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_commandRepositories.ContainsKey(type))
            {
                var repo = new SqlCommandRepository<T>(_context);
                _commandRepositories[type] = repo;
            }
            return (ISqlCommandRepository<T>)_commandRepositories[type];
        }

        public ISqlQueryRepository<T> QueryRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_queryRepositories.ContainsKey(type))
            {
                var repo = new SqlQueryRepository<T>(_context);
                _queryRepositories[type] = repo;
            }
            return (ISqlQueryRepository<T>)_queryRepositories[type];
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task ExecuteTransactionAsync(Func<Task> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await operation();
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
