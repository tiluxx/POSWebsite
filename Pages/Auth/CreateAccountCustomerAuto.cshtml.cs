using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using POSWebsite.Utils;

namespace POSWebsite.Pages.Auth
{
    public class CreateAccountCustomerAutoModel : PageModel
    {
        private readonly B2BDbContrext _dbContext;
        private readonly IVnPayController _vnPayController;
        private decimal _totalAmount = 2000000; // The safe limit to perform test on VNPay
        public List<CartItem> CartItems { get; set; }
        public Decimal Total { get; set; }

        public CreateAccountCustomerAutoModel(B2BDbContrext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty(SupportsGet = true)]
        public string PhoneNumber { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost(string phoneNumber, string customerName, string address, string gender, int branchStoreId, string deliveryAddress)
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
                var existingCustomer = _dbContext.Customer.FirstOrDefault(c => c.TelNo == phoneNumber);

                if (existingCustomer != null && branchStore != null)
                {
                    int orderId = 0;
                    CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
                    Total = CartItems.Sum(i => i.Product.RetailPrice * i.Quantity);
                    if (CartItems != null)
                    {
                        var order = new Order
                        {
                            DeliveryAddress = deliveryAddress,
                            TotalBill = Total,
                            Discount = 0,
                            ActualBill = Total * (100 - 0) / 100,
                            CreationDate = DateTime.Now,
                            Customer = existingCustomer,
                            CustomerId = existingCustomer.Id,
                            CreationLocation = branchStore,
                            CreationLocationId = branchStore.Id
                        };
                        OrderDetail orderDetail;
                        order.OrderDetails = new List<OrderDetail>();
                        Product product;
                        foreach (CartItem cartItem in CartItems)
                        {
                            product = _dbContext.Product.Find(cartItem.Product.Id);
                            orderDetail = new OrderDetail
                            {
                                Product = product,
                                Quantity = cartItem.Quantity,
                                Discount = 0,
                                ActualUnitPrice = cartItem.Product.RetailPrice * (100 - 0) / 100
                            };
                            order.OrderDetails.Add(orderDetail);
                        }
                        _dbContext.Order.Add(order);
                        _dbContext.SaveChanges();

                        orderId = order.Id;
                    }

                    Payment payment = new Payment(orderId.ToString(), _totalAmount);
                    string url = _vnPayController.CreatePaymentUrl(payment, HttpContext);

                    return Redirect(url);
                }
                else
                {
                    return RedirectToPage("/Error");
                }
            }

            return Page();
        }
    }
}
