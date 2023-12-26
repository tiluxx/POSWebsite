using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class Transaction
    {
        [Key]
        public string TransactionID { get; set; }
        public DateTime DateProcessed { get; set; }
        public Decimal TotalAmount { get; set; }
    }
}
