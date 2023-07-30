using KitsLib.Base.Models.Interfaces;

namespace KitsLib.Base.Models
{
    public record Order : IOrder
    {
        public Guid ID { get; set; }
        public Guid CustomerID { get; set; }
        public string Kit { get; set; }
        public ushort Amount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal Total { get; set; }
    }
}
