using Application.Common.Interfaces.Repositories.Persistence;
using Application.Filtering.Interfaces;
using Application.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class SqlQueryRepository<T> : ISqlQueryRepository<T> where T : class
    {
        protected readonly ApplicationDbContext context;
        protected readonly DbSet<T> dbSet;

        public SqlQueryRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
            => await dbSet.FindAsync(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync()
            => await dbSet.ToListAsync();

        public virtual async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> expression)
            => await dbSet.Where(expression).ToListAsync();

        public virtual async Task<IEnumerable<T>> GetFilteredAndPagedAsync(
            Expression<Func<T, bool>> filter,
            PaginationParameters paginationParams,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var query = GetQueryable(filter, asNoTracking, includeProperties)
                        .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                        .Take(paginationParams.PageSize);

            return await query.ToListAsync();
        }

        private IQueryable<T> GetQueryable(
            Expression<Func<T, bool>>? filter = null,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public async Task<PagedResult<T>> GetFilteredAndPagedAsync(
            List<IFilterStrategy<T>> filters,
            PaginationParameters pagination,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            foreach (var include in includes)
                query = query.Include(include);

            foreach (var filter in filters)
                query = query.Where(filter.ToExpression());

            int totalCount = await query.CountAsync();

            var result = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Data = result,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}
