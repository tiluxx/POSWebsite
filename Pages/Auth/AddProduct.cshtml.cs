using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop.Infrastructure;
using POSWebsite.Models;
using System;

namespace POSWebsite.Pages.Auth
{
    public class AddProductModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;

        [BindProperty]
        public Product Product { get; set; }

        public AddProductModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbContext.Product.Add(Product);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Auth/ListProduct");
        }

    }
}
