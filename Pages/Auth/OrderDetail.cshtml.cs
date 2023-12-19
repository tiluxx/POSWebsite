using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;

namespace POSWebsite.Pages.Auth
{
    public class OrderDetailModel : PageModel
    {
        private readonly ILogger<OrderDetailModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private Order _curOder;
        private List<OrderDetail> _orderDetails;

        public OrderDetailModel(ILogger<OrderDetailModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public Order GetCurOrder()
        {
            return _curOder;
        }

        public List<OrderDetail> GetOrderDetails()
        {
            return _orderDetails;
        }

        public void OnGet(string id)
        {
            Order? curOrder = _dbContext.Order.Where(o => o.Id.ToString() == id).FirstOrDefault();
            if (curOrder != null)
            {
                Customer? customer = _dbContext.Customer.Where(cust => cust.Id == curOrder.CustomerId).FirstOrDefault();
                BranchStore? branchStore = _dbContext.BranchStore.Where(branch => branch.Id == curOrder.CreationLocationId).FirstOrDefault();
                if (customer != null && branchStore != null)
                {
                    curOrder.Customer = customer;
                    curOrder.CreationLocation = branchStore;
                    _curOder = curOrder;
                }

                _orderDetails = _dbContext.OrderDetail.Where(od => od.OrderId.ToString() == id).ToList();
                foreach (OrderDetail orderDetail in _orderDetails)
                {
                    Product? productInOrder = _dbContext.Product.Where(p => p.Id == orderDetail.ProductId).FirstOrDefault();
                    if (productInOrder != null)
                    {
                        orderDetail.Product = productInOrder;
                    }
                }
            }
        }
    }
}
