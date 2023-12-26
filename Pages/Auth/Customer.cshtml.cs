using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSWebsite.Pages.Auth
{
    public class CustomerModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;
        private ResponseStatus _res;

        public CustomerModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public IList<Customer> Customers { get; set; }

        public async Task OnGetAsync()
        {
            Customers = await _dbContext.Customer.ToListAsync();
        }
    }
}
