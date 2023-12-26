using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;

namespace POSWebsite.Pages.Auth
{
    public class CustomerDetailsModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;

        public CustomerDetailsModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        public Customer Customer { get; set; }

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

            return Page();
        }
    }
}
