namespace POSWebsite.Models
{
    public class SalesReport
    {
        public string TypeOfTimeline { get; set; }
        public Decimal TotalAmount { get; set; }
        public long TotalOrders { get; set; }
        public long TotalProducts { get; set; }
        public long TotalCustomers { get; set; }
        public Decimal TotalProfit { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<RevenueItem> RevenueItems { get; set; } = new List<RevenueItem>();
    }
}
