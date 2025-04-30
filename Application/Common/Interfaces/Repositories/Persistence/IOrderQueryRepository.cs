using Domain.Entities;

namespace Application.Common.Interfaces.Repositories.Persistence
{
    public interface IOrderQueryRepository
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId, bool asNoTracking = false);
    }
}
