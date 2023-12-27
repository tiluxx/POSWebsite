using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;

namespace POSWebsite.Pages.Auth
{
    public class CreatAccountCustomerAutoModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;

        public CreatAccountCustomerAutoModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty(SupportsGet = true)]
        public string PhoneNumber { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost(string phoneNumber, string customerName, string address, string gender)
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

                var order = new Order
                {
                    CustomerId = newCustomer.Id,
                    DeliveryAddress = address
                };
                _dbContext.Order.Add(order);
                _dbContext.SaveChanges();

                return RedirectToPage("/Auth/PurchaseSuccess");
            }

            return Page();
        }
    }
}
