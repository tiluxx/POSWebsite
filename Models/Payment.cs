namespace POSWebsite.Models
{
    public class Payment
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }

        public Payment(string orderId, decimal amount)
        {
            OrderId = orderId;
            Amount = amount;
        }
    }
}
