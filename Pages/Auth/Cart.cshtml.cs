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
            CartItems = _dbContext.CartItems.ToList();
        }

        public IActionResult OnPostAddToCart(int productId, int quantity)
        {
            var existingCartItem = _dbContext.CartItems.FirstOrDefault(item => item.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                _dbContext.CartItems.Add(newCartItem);
            }

            _dbContext.SaveChanges();

            return RedirectToPage("/Auth/Cart");
        }
    }
}
