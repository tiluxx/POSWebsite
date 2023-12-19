namespace POSWebsite.Models
{
    public class ProductBranch
    {
        public int BranchId { get; set; }
        public BranchStore BranchStore { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
