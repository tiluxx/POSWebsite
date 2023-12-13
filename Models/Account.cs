using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace POSWebsite.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Fullname is required")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        [AllowNull]
        public string? ActivationToken { get; set; }
        [AllowNull]
        public bool IsActivated { get; set; }
        [AllowNull]
        public DateTime? TokenCreatedAt { get; set; }
        public bool IsNewPasswordCreated { get; set; }

        public Staff Staff { get; set; }
    }
}
