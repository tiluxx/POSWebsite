using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime DateJoined { get; set; }
        [Required]
        public string Gender { get; set; }
        public string BranchName { get; set; }
        public int YearOfWork { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public string TelNo { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsWorking { get; set; }
        public string ProfilePictureUrl { get; set; }

        public int BranchStoreId { get; set; }
        public BranchStore BranchStore { get; set; }
    }
}
