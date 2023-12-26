using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class CustomerLogin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
