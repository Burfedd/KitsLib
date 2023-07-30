namespace KitsLib.Base.Models.Interfaces
{
    public interface IOrder
    {
        public Guid ID { get; set; }
        public Guid CustomerID { get; set; }
        public string Kit { get; set; }
        public ushort Amount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal Total { get; set; }
    }
}
