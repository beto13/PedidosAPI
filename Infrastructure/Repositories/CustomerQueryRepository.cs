using Application.Common.Interfaces.Repositories.Persistence;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderQueryRepository : IOrderQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId, bool asNoTracking = false)
        {
            IQueryable<Order> query = _context.Set<Order>();

            if (asNoTracking)
                query = query.AsNoTracking();

            query = query.Include(o => o.OrderItems)
                         .ThenInclude(o => o.Product)
                         .Where(o => o.CustomerId == customerId);

            return await query.ToListAsync();
        }
    }
}
