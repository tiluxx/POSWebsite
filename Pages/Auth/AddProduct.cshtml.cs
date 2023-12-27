using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> OnPostAsync(IFormFile photo)
        {
            try
            {

                if (photo != null && photo.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "product", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    Product.Photo = "/images/product/" + fileName;
                }

                Product.CreationDate = DateTime.Now;

                _dbContext.Product.Add(Product);
                await _dbContext.SaveChangesAsync();

                var updateProducts = _dbContext.Product.ToListAsync();
                return RedirectToPage("/Auth/ListProduct", new { products = updateProducts });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }

    }
}
