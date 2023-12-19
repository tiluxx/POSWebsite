using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace POSWebsite.Models
{
    public class BranchStore
    {
        [Key]
        public int Id { get; set; } 
        public string BranchName { get; set; }
        public string Address { get; set; }
        public DateTime EstablishedDate { get; set; }
        public bool IsHeadCompany { get; set; } = false;

        public IList<ProductBranch> ProductBranches { get; set; }
        public ICollection<Staff> Staff { get; }
        public ICollection<Order> Orders { get; }
    }
}
