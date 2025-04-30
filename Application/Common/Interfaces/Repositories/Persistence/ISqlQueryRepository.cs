using Application.Filtering.Interfaces;
using Application.Models;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Repositories.Persistence
{
    public interface ISqlQueryRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetFilteredAndPagedAsync(
            Expression<Func<T, bool>> filter,
            PaginationParameters paginationParams,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] includeProperties);

        Task<PagedResult<T>> GetFilteredAndPagedAsync(
             List<IFilterStrategy<T>> filters,
             PaginationParameters pagination,
             bool asNoTracking = false,
             params Expression<Func<T, object>>[] includes);
    }
}
