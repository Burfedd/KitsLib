using KitsLib.Base.Models;
using KitsLib.Base.Models.Interfaces;
using KitsLib.Base.Repositories.Interfaces;
using KitsLib.Base.Services;
using Moq;
using NUnit.Framework;

namespace KitsLib.Tests.Services
{
    [TestFixture]
    public class OrderPlacementServiceTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IKitRepository> _kitRepositoryMock;
        private Mock<OrderPlacementService> _orderPlacementServiceMock;

        [SetUp]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _kitRepositoryMock = new Mock<IKitRepository>();
            _orderPlacementServiceMock = new Mock<OrderPlacementService>(_orderRepositoryMock.Object, _kitRepositoryMock.Object);
        }

        [TestCase((ushort)0)]
        [TestCase((ushort)1000)]
        public void GivenInvalidAmount_WhenPlaceOrder_ThenNull(ushort amount)
        {
            // Arrange
            Guid customerGuid = Guid.NewGuid();
            DateTime deliveryDate = DateTime.Now.AddDays(2);
            string kitName = "kitName";

            // Act
            Task<IOrder> resultTask = _orderPlacementServiceMock.Object.PlaceOrder(amount, customerGuid, deliveryDate, kitName);

            // Assert
            Assert.IsNull(resultTask.Result);
        }

        [Test]
        public void GivenPastDeliveryDate_WhenPlaceOrder_ThenNull()
        {
            // Arrange
            DateTime deliveryDate = DateTime.Now.AddDays(-1);
            Guid customerGuid = Guid.NewGuid();
            string kitName = "kitName";
            ushort amount = 1;

            // Act
            Task<IOrder> resultTask = _orderPlacementServiceMock.Object.PlaceOrder(amount, customerGuid, deliveryDate, kitName);

            // Assert
            Assert.IsNull(resultTask.Result);
        }

        [Test]
        public void GivenEmptyCustomerId_WhenPlaceOrder_ThenNull()
        {
            // Arrange
            DateTime deliveryDate = DateTime.Now.AddDays(-1);
            Guid customerGuid = Guid.Empty;
            string kitName = "kitName";
            ushort amount = 1;

            // Act
            Task<IOrder> resultTask = _orderPlacementServiceMock.Object.PlaceOrder(amount, customerGuid, deliveryDate, kitName);

            // Assert
            Assert.IsNull(resultTask.Result);
        }

        [Test]
        public void GivenCorrectData_WhenPlaceOrder_ThenInsert()
        {
            // Arrange
            DateTime deliveryDate = DateTime.Now.AddDays(1);
            Guid customerGuid = Guid.NewGuid();
            string kitName = "kitName";
            ushort amount = 1;

            Mock<BasicKit> kitMock = new Mock<BasicKit>();
            kitMock.SetupAllProperties();

            Mock<Order> orderMock = new Mock<Order>();
            orderMock.Object.ID = Guid.NewGuid();
            orderMock.Object.Amount = amount;
            orderMock.Object.CustomerID = customerGuid;
            orderMock.Object.DeliveryDate = deliveryDate;
            orderMock.Object.Kit = kitName;
            orderMock.Object.Total = 15m;

            _kitRepositoryMock.Setup(r => r.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(kitMock.Object).Verifiable();
            _orderRepositoryMock.Setup(r => r.InsertAsync(It.IsAny<Order>())).ReturnsAsync(orderMock.Object).Verifiable();

            // Act
            Task<IOrder> resultTask = _orderPlacementServiceMock.Object.PlaceOrder(amount, customerGuid, deliveryDate, kitName);

            // Assert
            Assert.That(resultTask.Result, Is.SameAs(orderMock.Object));
        }

        [TestCase((ushort)200, 16828.3)]
        [TestCase((ushort)100, 8414.15)]
        [TestCase((ushort)50, 4207.08)]
        [TestCase((ushort)25, 2351.02)]
        [TestCase((ushort)20, 1880.81)]
        [TestCase((ushort)10, 940.41)]
        [TestCase((ushort)5, 494.95)]
        [TestCase((ushort)1, 98.99)]
        public void GivenAmounts_WhenPlaceOrder_ThenCorrectDiscount(ushort amount, decimal expectedResult)
        {
            // Arrange
            IOrder capturedOrder = null;
            _orderRepositoryMock.Setup(r => r.InsertAsync(It.IsAny<IOrder>())).Callback<IOrder>(order => capturedOrder = order);

            DateTime deliveryDate = DateTime.Now.AddDays(1);
            Guid customerGuid = Guid.NewGuid();
            string kitName = "kitName";

            Mock<BasicKit> kitMock = new Mock<BasicKit>();
            kitMock.SetupAllProperties();

            _kitRepositoryMock.Setup(r => r.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(kitMock.Object);

            // Act
            Task<IOrder> resultTask = _orderPlacementServiceMock.Object.PlaceOrder(amount, customerGuid, deliveryDate, kitName);

            // Assert
            Assert.IsNotNull(capturedOrder);
            Assert.That(expectedResult, Is.EqualTo(capturedOrder.Total));
            Assert.IsNull(resultTask.Result);
        }

        [Test]
        public void GivenEmptyCustomerId_WhenListOrders_ThenNull()
        {
            // Arrange
            Guid customerGuid = Guid.Empty;

            // Act
            Task<IEnumerable<IOrder>> result = _orderPlacementServiceMock.Object.ListOrders(customerGuid);

            // Assert
            Assert.IsNull(result.Result);
        }

        [Test]
        public void GivenCorrectCustomerId_WhenListOrders_ThenList()
        {
            // Arrange
            Guid customerGuid = Guid.NewGuid();
            IEnumerable<IOrder> orders = new List<IOrder>();

            _orderRepositoryMock.Setup(r => r.GetOrdersByCustomerIDAsync(It.IsAny<Guid>())).ReturnsAsync(orders);

            // Act
            Task<IEnumerable<IOrder>> result = _orderPlacementServiceMock.Object.ListOrders(customerGuid);

            // Assert
            Assert.That(result.Result, Is.SameAs(orders));
        }
    }
}
