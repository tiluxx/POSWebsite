using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string DeliveryAddress { get; set; }
        public Decimal TotalBill { get; set; }
        public Decimal Discount { get; set; }
        public Decimal ActualBill { get; set; }
        public DateTime CreationDate { get; set; }

        public Customer Customer { get; set; }
        public BranchStore CreationLocation { get; set; }

        public IList<OrderDetail> OrderDetails { get; set; }
    }
}
