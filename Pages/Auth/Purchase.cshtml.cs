using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.Linq;

namespace POSWebsite.Pages.Auth
{
    public class PurchaseModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;

        public PurchaseModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty(SupportsGet = true)]
        public int SelectedProductId { get; set; }

        public Product SelectedProduct { get; set; }

        public List<BranchStore> BranchStores { get; set; }

        public IActionResult OnGet()
        {
            SelectedProduct = _dbContext.Product.Find(SelectedProductId);

            if (SelectedProduct == null)
            {
                return RedirectToPage("/Error");
            }
            BranchStores = _dbContext.BranchStore.ToList();
            return Page();
        }

        public IActionResult OnPost(string phoneNumber, string deliveryAddress, int branchStoreId)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(deliveryAddress))
            {
                var existingCustomer = _dbContext.Customer.FirstOrDefault(c => c.TelNo == phoneNumber);

                if (existingCustomer != null)
                {
                    var order = new Order
                    {
                        CustomerId = existingCustomer.Id,
                        DeliveryAddress = deliveryAddress,
                        CreationLocationId = branchStoreId
                    };
                    _dbContext.Order.Add(order);
                    _dbContext.SaveChanges();
                }
                else
                {
                    return RedirectToPage("/Auth/CreatAccountCustomerAuto", new { phoneNumber = phoneNumber, deliveryAddress = deliveryAddress });
                }

                return RedirectToPage("/Auth/PurchaseSuccess");
            }

            return RedirectToPage("/Auth/Purchase");
        } 
    }
}
