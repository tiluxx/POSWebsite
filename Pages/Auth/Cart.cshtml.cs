using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.Collections.Generic;
using System.Linq;

namespace POSWebsite.Pages.Auth
{
    public class CartModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;

        public CartModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CartItem> CartItems { get; set; }

        public void OnGet()
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
            if (CartItems == null)
            {
                CartItems = new List<CartItem>();
            }
        }

        public IActionResult OnGetBuyNow(int productId)
        {
            var product = _dbContext.Product.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                var cartItem = new CartItem
                {
                    Product = product,
                    ProductId = product.Id,
                    Quantity = 1
                };

                CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");

                if (CartItems == null)
                {
                    CartItems = new List<CartItem>();
                }

                CartItems.Add(cartItem);

                SessionHelper.SetObjectAsJson(HttpContext.Session, "CartItems", CartItems);
            }

            return RedirectToPage("Auth/Cart"); // Chuyển hướng đến trang Order
        }
    }
}
