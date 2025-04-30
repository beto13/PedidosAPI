using Domain.Entities;

namespace Application.Common.Interfaces.Repositories.Persistence
{
    public interface ICustomerQueryRepository
    {
        Task<Customer> GetCustomerWithOrdersByIdAsync(Guid customerId, bool asNoTracking = false);
    }
}
