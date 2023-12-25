using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Barcode is required")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        public string UnitPrice { get; set; }

        [Required(ErrorMessage = "Import price is required")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Import price must be a positive number")]
        public Decimal ImportPrice { get; set; }

        [Required(ErrorMessage = "Retail price is required")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Retail price must be a positive number")]
        public Decimal RetailPrice { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public string Photo { get; set; }

        public IList<OrderDetail> OrderDetails { get; set; }
        public IList<ProductBranch> ProductBranches { get; set; }
    }
}
