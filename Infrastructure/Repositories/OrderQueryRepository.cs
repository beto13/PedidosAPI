using Application.Common.Interfaces.Repositories.Persistence;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerQueryRepository : ICustomerQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerWithOrdersByIdAsync(Guid customerId, bool asNoTracking = false)
        {
            IQueryable<Customer> query = _context.Set<Customer>();

            if (asNoTracking)
                query = query.AsNoTracking();

            query = query.Include(c => c.Orders)
                         .ThenInclude(o => o.OrderItems)
                         .Where(c => c.Id == customerId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
