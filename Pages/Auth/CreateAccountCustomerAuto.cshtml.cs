using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;

namespace POSWebsite.Pages.Auth
{
    public class CreateAccountCustomerAutoModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;

        public CreateAccountCustomerAutoModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty(SupportsGet = true)]
        public string PhoneNumber { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost(string phoneNumber, string customerName, string address, string gender, int branchStoreId)
        {
            if (!string.IsNullOrEmpty(phoneNumber) &&
                !string.IsNullOrEmpty(customerName) &&
                !string.IsNullOrEmpty(address) &&
                !string.IsNullOrEmpty(gender))
            {
                var newCustomer = new Customer
                {
                    TelNo = phoneNumber,
                    Fullname = customerName,
                    Address = address,
                    Gender = gender
                };
                _dbContext.Customer.Add(newCustomer);
                _dbContext.SaveChanges();

                var branchStore = _dbContext.BranchStore.FirstOrDefault(b => b.Id == branchStoreId);

                if (branchStore != null)
                {
                    var order = new Order
                    {
                        CustomerId = newCustomer.Id,
                        DeliveryAddress = address,
                        CreationLocationId = branchStoreId
                    };
                    _dbContext.Order.Add(order);
                    _dbContext.SaveChanges();

                    return RedirectToPage("/Error");
                }
                else
                {
                    return RedirectToPage("/Auth/PurchaseSuccess");
                }
            }

            return Page();
        }

    }
}
