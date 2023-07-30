using KitsLib.Base.Models.Interfaces;
using KitsLib.Base.Repositories.Interfaces;

namespace KitsLib.SampleApp.Repositories
{
    public class OrdersRepository : IOrderRepository
    {
        private static List<IOrder> _storage = new();

        public Task<IEnumerable<IOrder>> GetAllAsync()
        {
            return Task.FromResult(_storage as IEnumerable<IOrder>);
        }

        public Task<IEnumerable<IOrder>> GetOrdersByCustomerIDAsync(Guid customerId)
        {
            return Task.FromResult(_storage.Select(order => order).Where(o => o.CustomerID == customerId));
        }

        public Task<IOrder> InsertAsync(IOrder entity)
        {
            _storage.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<IOrder>> InsertRangeAsync(IEnumerable<IOrder> entities)
        {
            _storage.AddRange(entities);
            return Task.FromResult(_storage as IEnumerable<IOrder>);
        }
    }
}
