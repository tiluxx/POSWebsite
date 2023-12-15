using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string UnitPrice { get; set; }
        public Decimal ImportPrice { get; set; }
        public Decimal RetailPrice { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public BranchStore BranchStore { get; set; }

        public IList<OrderDetail> OrderDetails { get; set; }
    }
}
