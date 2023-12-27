using Azure.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using POSWebsite.Models;

namespace POSWebsite.Pages.Auth
{
    public class OrderModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;

        public OrderModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CartItem> CartItems { get; set; }

        public void OnGet()
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
        }
    }
}
