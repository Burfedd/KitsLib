using KitsLib.Base.Models;
using KitsLib.Base.Models.Interfaces;
using KitsLib.Base.Repositories.Interfaces;
using KitsLib.Base.Services.Interfaces;

namespace KitsLib.Base.Services
{
    public class OrderPlacementService : IOrderPlacementService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IKitRepository _kitRepository;

        /// <summary>
        /// Initializes a new instance of <see cref="OrderPlacementService"/> class
        /// </summary>
        /// <param name="orderRepository">Data access layer for <see cref="IOrder"/> objects</param>
        /// <param name="kitRepository">Data access layer for <see cref="IKit"/> objects</param>
        public OrderPlacementService(IOrderRepository orderRepository, IKitRepository kitRepository)
        {
            _orderRepository = orderRepository;
            _kitRepository = kitRepository;
        }

        /// <summary>
        /// Returns all orders placed by a customer with specified ID
        /// </summary>
        /// <param name="customerId">A unique identifier of a customer</param>
        /// <returns>A list of <see cref="IOrder"/> objects associated with a customer, or <see cref="null"/> if GUID is empty</returns>
        public async Task<IEnumerable<IOrder>> ListOrders(Guid customerId)
        {
            if (customerId != Guid.Empty)
            {
                return await _orderRepository.GetOrdersByCustomerIDAsync(customerId);
            }
            return null;
        }

        /// <summary>
        /// Places an <see cref="Order"/> after validating input data
        /// </summary>
        /// <param name="amount">Amount of kits ordered</param>
        /// <param name="customerId">Unique customer identifier</param>
        /// <param name="deliveryDate">Date to be delivered on</param>
        /// <param name="kitName">Name of the kit(-s) ordered</param>
        /// <returns>An <see cref="Order"/> object if the order placement process was successful, otherwise - <see cref="null"/></returns>
        public async Task<IOrder> PlaceOrder(ushort amount, Guid customerId, DateTime deliveryDate, string kitName)
        {
            if (IsValidDeliveryDate(deliveryDate) && IsValidAmount(amount) && customerId != Guid.Empty)
            {
                IKit kit = await _kitRepository.FindByNameAsync(kitName);
                if (kit is not null)
                {
                    return await _orderRepository.InsertAsync(new Order()
                    {
                        ID = Guid.NewGuid(),
                        CustomerID = customerId,
                        Kit = kitName,
                        Amount = amount,
                        DeliveryDate = deliveryDate,
                        Total = CalculateTotal(kit.BasePrice, amount)
                    });
                }
            }
            return null;
        }

        private bool IsValidDeliveryDate(DateTime date) => DateTime.Now < date;
        private bool IsValidAmount(ushort amount) => 0 < amount && amount < 999;
        private decimal CalculateTotal(decimal basePrice, ushort amount) => amount switch
        {
            >= 50 => Math.Round(basePrice * amount * 0.85m, 2, MidpointRounding.ToPositiveInfinity),
            >= 10 => Math.Round(basePrice * amount * 0.95m, 2, MidpointRounding.ToPositiveInfinity),
            _ => Math.Round(basePrice * amount, 2, MidpointRounding.ToPositiveInfinity)
        };
    }
}
