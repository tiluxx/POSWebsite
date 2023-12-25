using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace POSWebsite.Pages.Auth
{
    public class ListProductModel : PageModel
    {
        private readonly ILogger<ListProductModel> _logger;
        private readonly B2BDbContrext _dbContext;

        public ListProductModel(ILogger<ListProductModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IList<Models.Product> Products { get; set; }

        public void OnGet()
        {
            Products = _dbContext.Product.ToList() ?? new List<Product>();
        }


        public IActionResult OnGetEditProduct(int productId)
        {
            return RedirectToPage("/Auth/EditProduct", new { id = productId });
        }

        public async Task<IActionResult> OnPostDeleteProduct(int productId)
        {
            var product = _dbContext.Product.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Auth/ListProduct");
        }

    }
}