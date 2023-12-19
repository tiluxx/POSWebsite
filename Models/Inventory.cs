namespace POSWebsite.Models
{
    public class Inventory
    {
        public Product Product { get; set; }
        public List<ProductBranch> Branches { get; set; }
        public ProductBranch Branch { get; set; }
    }
}
