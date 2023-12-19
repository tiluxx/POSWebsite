using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string TelNo { get; set; }
        public string Address { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<Order> Orders { get; }
    }
}
