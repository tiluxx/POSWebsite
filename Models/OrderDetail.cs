using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Decimal Discount { get; set; }
        public Decimal ActualUnitPrice { get; set; }
    }
}
