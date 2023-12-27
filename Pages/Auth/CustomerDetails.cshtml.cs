using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;
using System.Linq;
using static NuGet.Packaging.PackagingConstants;

namespace POSWebsite.Pages.Auth
{
    public class CustomerDetailsModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;
        private ResponseStatus _res;

        public CustomerDetailsModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        public Customer Customer { get; set; }
        public List<Order> Orders { get; set; }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = await _dbContext.Customer.FirstOrDefaultAsync(m => m.Id == id);
            if (Customer == null)
            {
                return NotFound();
            }

            Orders = _dbContext.Order.Where(order => order.CustomerId == Customer.Id).ToList();

            return Page();
        }
    }
}
