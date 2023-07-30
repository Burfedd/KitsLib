using KitsLib.Base.Models.Interfaces;

namespace KitsLib.Base.Models
{
    public class BasicKit : IKit
    {
        public string Name { get; set; } = "BasicKit";
        public decimal BasePrice { get; set; } = 98.99m;
    }
}
