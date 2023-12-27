using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using POSWebsite.Models;
using System.Web.WebPages.Html;
using Microsoft.EntityFrameworkCore;

public class EditProductModel : PageModel
{
    private readonly B2BDbContrext _dbContext;
    private readonly ILogger<EditProductModel> _logger;

    /*[BindProperty]
    public Product Product { get; set; }*/
    public Product Product { get; set; }

    public EditProductModel(ILogger<EditProductModel> logger, B2BDbContrext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IActionResult OnGet(int id)
    {

        Product = _dbContext.Product.FirstOrDefault(p => p.Id == id);

        if (Product == null)
        {

            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string productName, string barcode, string retailPrice, string category, string description, string productId)
    {
        /*ModelState.Remove("Product.OrderDetails");
        ModelState.Remove("Product.ProductBranches");*/

        if (!ModelState.IsValid)
        {
            /*foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError($"ModelState Error: {error.ErrorMessage}");
                }
            }*/

            return Page();
        }

        try
        {
            var existingProduct = await _dbContext.Product.Where(p => p.Id.ToString() == productId).FirstOrDefaultAsync();

            if (existingProduct != null)
            {
                existingProduct.ProductName = productName;
                existingProduct.Barcode = barcode;
                existingProduct.RetailPrice = Convert.ToDecimal(retailPrice);
                existingProduct.Category = category;
                existingProduct.Description = description;

                _dbContext.Product.Update(existingProduct);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("The product is updated successfully!");

                return RedirectToPage("/Auth/ListProduct");
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred when we' trying to update this product!");
            throw;
        }
    }

}
