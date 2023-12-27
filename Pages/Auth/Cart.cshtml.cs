using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

namespace POSWebsite.Pages.Auth
{
    public class CartModel : PageModel
    {
        private readonly ILogger<CreateStaffAccountModel> _logger;
        private readonly B2BDbContrext _dbContext;

        public CartModel(ILogger<CreateStaffAccountModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public List<CartItem> CartItems { get; set; }
        public Decimal Total { get; set; }

        public void OnGet()
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
            if (CartItems == null)
            {
                CartItems = new List<CartItem>();
            } else
            {
                Total = CartItems.Sum(i => i.Product.RetailPrice * i.Quantity);
            }
        }

        public IActionResult OnGetBuyNow(string id)
        {
            var product = _dbContext.Product.FirstOrDefault(p => p.Id.ToString() == id);

            if (product != null)
            {
                var cartItem = new CartItem
                {
                    Product = product,
                    Quantity = 1
                };

                CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");

                if (CartItems == null)
                {
                    CartItems = new List<CartItem>
                    {
                        cartItem
                    };

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "CartItems", CartItems);
                } else
                {
                    int index = Exists(CartItems, id);
                    if (index == -1)
                    {
                        CartItems.Add(cartItem);
                    }
                    else
                    {
                        CartItems[index].Quantity++;
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "CartItems", CartItems);
                }
            }

            return RedirectToPage("/Auth/Cart");
        }

        public IActionResult OnGetDelete(string id)
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
            int index = Exists(CartItems, id);
            CartItems.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CartItems", CartItems);
            return RedirectToPage("/Auth/Cart");
        }

        public IActionResult OnPostUpdate(int[] quantities)
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
            for (var i = 0; i < CartItems.Count; i++)
            {
                CartItems[i].Quantity = quantities[i];
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CartItems", CartItems);
            return RedirectToPage("/Auth/Cart");
        }

        private int Exists(List<CartItem> cart, string id)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.ToString() == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
