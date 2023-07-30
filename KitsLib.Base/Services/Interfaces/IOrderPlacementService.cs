using KitsLib.Base.Models.Interfaces;

namespace KitsLib.Base.Services.Interfaces
{
    public interface IOrderPlacementService
    {
        Task<IOrder> PlaceOrder(ushort amount, Guid customerId, DateTime deliveryDate, string kitName);
        Task<IEnumerable<IOrder>> ListOrders(Guid customerId);
    }
}
