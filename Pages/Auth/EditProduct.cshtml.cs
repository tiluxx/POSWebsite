using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using POSWebsite.Models;
using System.Web.WebPages.Html;
using Microsoft.EntityFrameworkCore;

public class EditProductModel : PageModel
{
    private readonly B2BDbContrext _dbContext;
    private readonly ILogger<EditProductModel> _logger;

    [BindProperty]
    public Product Product { get; set; }

    public EditProductModel(B2BDbContrext dbContext, ILogger<EditProductModel> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IActionResult OnGet(int id)
    {
        _logger.LogInformation($"Received Id: {id}");

        Product = _dbContext.Product.FirstOrDefault(p => p.Id == id);

        if (Product == null)
        {
            _logger.LogWarning($"Product with Id {id} does not exist.");

            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Product.OrderDetails");
        ModelState.Remove("Product.ProductBranches");

        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError($"ModelState Error: {error.ErrorMessage}");
                }
            }

            return Page();
        }

        _logger.LogInformation($"Received ProductName from form: {Product.ProductName}");

        try
        {
            var existingProduct = await _dbContext.Product
                .Where(p => p.Id == Product.Id)
                .Select(p => new Product
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Barcode = p.Barcode,
                    UnitPrice = p.UnitPrice,
                    ImportPrice = p.ImportPrice,
                    RetailPrice = p.RetailPrice,
                    Category = p.Category,
                    Description = p.Description,
                    Photo = p.Photo
                })
                .FirstOrDefaultAsync();

            if (existingProduct != null)
            {
                existingProduct.ProductName = Product.ProductName;
                existingProduct.Barcode = Product.Barcode;
                existingProduct.UnitPrice = Product.UnitPrice;
                existingProduct.ImportPrice = Product.ImportPrice;
                existingProduct.RetailPrice = Product.RetailPrice;
                existingProduct.Category = Product.Category;
                existingProduct.Description = Product.Description;
                existingProduct.Photo = Product.Photo;

                _dbContext.Product.Update(existingProduct);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Dữ liệu đã được cập nhật thành công!");

                return RedirectToPage("/Auth/ListProduct");
            }
            else
            {
                _logger.LogWarning($"Product with Id {Product.Id} does not exist.");

                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật dữ liệu!");
            throw;
        }
    }

}
