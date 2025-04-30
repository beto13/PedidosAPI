using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Repositories.Persistence
{
    public interface ISqlCommandRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Remove(T entity);
        void Update(T entity);
        void UpdateProperties(T entity, params Expression<Func<T, object>>[] properties);
        Task SoftDeleteAsync(T entity);
    }
}
