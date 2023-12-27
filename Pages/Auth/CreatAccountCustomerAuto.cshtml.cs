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

        public IActionResult OnPost(string customerName, string address, string gender)
        {
            if (!string.IsNullOrEmpty(PhoneNumber) &&
                !string.IsNullOrEmpty(customerName) &&
                !string.IsNullOrEmpty(address) &&
                !string.IsNullOrEmpty(gender)) // Ensure gender is not null or empty
            {
                var newCustomer = new Customer
                {
                    TelNo = PhoneNumber,
                    Fullname = customerName,
                    Address = address,
                    Gender = gender  // Assign the gender value
                };
                _dbContext.Customer.Add(newCustomer);
                _dbContext.SaveChanges();

                var order = new Order
                {
                    CustomerId = newCustomer.Id,
                    DeliveryAddress = address // Assign the delivery address value
                };
                _dbContext.Order.Add(order);
                _dbContext.SaveChanges();

                return RedirectToPage("/Auth/PurchaseSuccess");
            }

            return Page();
        }


    }
}
