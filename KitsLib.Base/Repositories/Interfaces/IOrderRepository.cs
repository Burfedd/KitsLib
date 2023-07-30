using KitsLib.Base.Models.Interfaces;

namespace KitsLib.Base.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<IOrder>
    {
        Task<IEnumerable<IOrder>> GetOrdersByCustomerIDAsync(Guid customerId);
    }
}
