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

        public List<Product> CartProducts { get; set; }

        public void OnGet()
        {
            var cartProducts = Request.Cookies["CartProducts"];

            if (!string.IsNullOrEmpty(cartProducts))
            {
                var productList = JsonConvert.DeserializeObject<List<string>>(cartProducts);

                // Dựa vào danh sách ID sản phẩm từ cookies để truy vấn và lấy thông tin sản phẩm từ database
                CartProducts = _dbContext.Product.Where(p => productList.Contains(p.Id.ToString())).ToList();
            }
            else
            {
                CartProducts = new List<Product>();
            }
        }
    }
}
