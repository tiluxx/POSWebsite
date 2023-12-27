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
        private readonly B2BDbContrext _dbContext;

        public CartModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CartItem> CartItems { get; set; }

        public void OnGet()
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
        }

        public IActionResult OnPostUpdate(int[] quantities)
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");

            for (var i = 0; i < CartItems.Count; i++)
            {
                CartItems[i].Quantity = quantities[i];
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "CartItems", CartItems);
            return RedirectToPage("/Auth/Order");
        }
    }
}
