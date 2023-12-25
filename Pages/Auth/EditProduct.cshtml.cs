using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using POSWebsite.Models;
using System.Web.WebPages.Html;

public class EditProductModel : PageModel
{
    private readonly B2BDbContrext _dbContext;

    [BindProperty]
    public Product Product { get; set; }

    public EditProductModel(B2BDbContrext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _dbContext.Product.Update(Product);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage("/Auth/ListProduct");
    }
}
