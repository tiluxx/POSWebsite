using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IList<Product> Products { get; set; }

        /*public void OnGet(List<Product> products = null)
        {
            if (products != null && products.Any())
            {
                Products = products;
            }
            else
            {
                Products = _dbContext.Product.ToList() ?? new List<Product>();
            }
        }*/

        public void OnGet()
        {
            Products = _dbContext.Product.ToList() ?? new List<Product>();
        }

        /* [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        public async Task OnGetAsync(List<Product> products = null, string searchName = null)
        {
            IQueryable<Product> productsQuery = _dbContext.Product;

            if (products != null && products.Any())
            {
                Products = products;
            }
            else
            {
                if (!string.IsNullOrEmpty(searchName))
                {
                    // Filter products by name using case-insensitive search
                    productsQuery = productsQuery.Where(p => p.ProductName.Contains(searchName, StringComparison.OrdinalIgnoreCase));
                }

                Products = await productsQuery.ToListAsync();
            }
        } */



        public IActionResult OnGetEditProduct(int productId)
        {
            return RedirectToPage("/Auth/EditProduct", new { id = productId });
        }

        public async Task<IActionResult> OnPost(string productId)
        {
            /*var product = _dbContext.Product.FirstOrDefault(p => p.Id.To == productId);

            if (product == null)
            {
                return NotFound();
            }*/

            _dbContext.ProductBranch.Remove(
                _dbContext.ProductBranch.Single(
                    productBranch => productBranch.ProductId.ToString() == productId));
            _dbContext.Product.Remove(_dbContext.Product.Single(product => product.Id.ToString() == productId));
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Auth/ListProduct");
        }
    }
}