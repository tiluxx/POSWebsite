using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSWebsite.Models;

namespace POSWebsite.Pages.Auth
{
    public class AddProductBarcodeModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;

        public AddProductBarcodeModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        public Product Product { get; set; }

        [BindProperty]
        public string Barcode { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostBarcodeAsync(string barcode)
        {
            try
            {
                var existingProduct = await _dbContext.Product.FirstOrDefaultAsync(p => p.Barcode == barcode);

                if (existingProduct == null)
                {
                    ModelState.AddModelError("Product.Barcode", "Product with this barcode does not exist.");
                    return Page();
                }

                // Lấy danh sách sản phẩm từ cookie
                var cartProducts = Request.Cookies["CartProducts"];
                var productList = new List<string>();

                if (!string.IsNullOrEmpty(cartProducts))
                {
                    productList = JsonConvert.DeserializeObject<List<string>>(cartProducts);
                }

                // Thêm mã sản phẩm vào danh sách sản phẩm
                productList.Add(existingProduct.Id.ToString());

                // Lưu danh sách sản phẩm vào cookie
                Response.Cookies.Append("CartProducts", JsonConvert.SerializeObject(productList));

                return RedirectToPage("/Auth/Order");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }

    }
}
