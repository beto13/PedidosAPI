using Application.Common.Interfaces.Repositories.Persistence;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    internal class SqlCommandRepository<T> : ISqlCommandRepository<T> where T : class
    {

        protected readonly ApplicationDbContext context;
        protected readonly DbSet<T> dbSet;

        public SqlCommandRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
         => await dbSet.AddAsync(entity);

        public virtual void Remove(T entity)
            => dbSet.Remove(entity);

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateProperties(T entity, params Expression<Func<T, object>>[] properties)
        {
            dbSet.Attach(entity);
            foreach (var property in properties)
            {
                context.Entry(entity).Property(property).IsModified = true;
            }
        }

        public async Task SoftDeleteAsync(T entity)
        {
            if (entity is BaseEntity deletableEntity)
            {
                deletableEntity.DeletedAt = DateTime.UtcNow;
                dbSet.Update(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
