using KitsLib.Base.Models;
using KitsLib.Base.Models.Interfaces;
using KitsLib.Base.Services;
using KitsLib.SampleApp.Repositories;

namespace KitsLib.SampleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            OrdersRepository ordersRepo = new OrdersRepository();
            KitsRepository kitsRepo = new KitsRepository();
            OrderPlacementService service = new OrderPlacementService(ordersRepo, kitsRepo);

            kitsRepo.InsertAsync(new BasicKit());
            IEnumerable<IKit> kits = kitsRepo.GetAllAsync().Result;
            foreach(IKit kit in kits)
            {
                await Console.Out.WriteLineAsync($"{Environment.NewLine} [KIT] Name: {kit.Name}, Base price: {kit.BasePrice}");
            }

            Guid first = Guid.NewGuid();
            Guid second = Guid.NewGuid();
            Guid third = Guid.NewGuid();

            await service.PlaceOrder(3, first, DateTime.Now.AddDays(2), "BasicKit");
            await service.PlaceOrder(36, first, DateTime.Now.AddDays(6), "BasicKit");
            await service.PlaceOrder(12, second, DateTime.Now.AddDays(1), "BasicKit");
            await service.PlaceOrder(5, second, DateTime.Now.AddDays(4), "BasicKit");
            await service.PlaceOrder(205, second, DateTime.Now.AddDays(67), "BasicKit");
            await service.PlaceOrder(100, second, DateTime.Now.AddDays(2), "BasicKit");
            await service.PlaceOrder(102, second, DateTime.Now.AddDays(4), "BasicKit");
            await service.PlaceOrder(200, third, DateTime.Now.AddDays(15), "BasicKit");

            await Console.Out.WriteLineAsync("First customer's orders:");
            foreach (IOrder order in service.ListOrders(first).Result)
            {
                await Console.Out.WriteLineAsync($"{Environment.NewLine} [ORDER] Kit name: {order.Kit}, Amount: {order.Amount}, Total: {order.Total}");
            }

            await Console.Out.WriteLineAsync("Second customer's orders:");
            foreach (IOrder order in service.ListOrders(second).Result)
            {
                await Console.Out.WriteLineAsync($"{Environment.NewLine} [ORDER] Kit name: {order.Kit}, Amount: {order.Amount}, Total: {order.Total}");
            }

            await Console.Out.WriteLineAsync("Third customer's orders:");
            foreach (IOrder order in service.ListOrders(third).Result)
            {
                await Console.Out.WriteLineAsync($"{Environment.NewLine} [ORDER] Kit name: {order.Kit}, Amount: {order.Amount}, Total: {order.Total}");
            }
        }
    }
}