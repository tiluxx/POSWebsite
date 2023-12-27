using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using POSWebsite.Models;
using System.Net;

namespace POSWebsite.Pages.Auth
{
    public class OrderModel : PageModel
    {
        private readonly ILogger<OrderModel> _logger;
        private readonly B2BDbContrext _dbContext;

        public OrderModel(ILogger<OrderModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public List<CartItem> CartItems { get; set; }
        public Decimal Total { get; set; }
        public List<BranchStore> BranchStores { get; set; }

        public void OnGet()
        {
            BranchStores = _dbContext.BranchStore.ToList();
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
            if (CartItems == null)
            {
                CartItems = new List<CartItem>();
            }
            else
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
                }
                else
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

            return RedirectToPage("/Auth/Order");
        }

        public IActionResult OnGetDelete(string id)
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "CartItems");
            int index = Exists(CartItems, id);
            CartItems.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CartItems", CartItems);
            return RedirectToPage("/Auth/Order");
        }

        public IActionResult OnPostPurchase(string phoneNumber, string deliveryAddress, string branchStoreId)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(deliveryAddress))
            {
                var existingCustomer = _dbContext.Customer.FirstOrDefault(c => c.TelNo == phoneNumber);
                var branchStore = _dbContext.BranchStore.FirstOrDefault(b => b.Id.ToString() == branchStoreId);

                if (existingCustomer != null && branchStore != null)
                {
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
                    }
                }
                else
                {
                    return RedirectToPage("/Auth/CreateAccountCustomerAuto", new { phoneNumber, customerName = "", address = "", gender = "", branchStoreId, deliveryAddress });
                }
            }

            return RedirectToPage("/Auth/PurchaseSuccess");
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
