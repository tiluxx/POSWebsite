using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using POSWebsite.Pages.Auth;
using System.Linq;

namespace POSWebsite.Pages.HeadQuarter
{
    public class RevenueStatisticsModel : PageModel
    {
        private readonly ILogger<RevenueStatisticsModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private List<BranchStore> _branches;

        public RevenueStatisticsModel(ILogger<RevenueStatisticsModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public List<BranchStore> GetBranches()
        {
            return _branches;
        }

        public void OnGet(bool Status, string Message, string AgentSelection, string FromDate, string ToDate)
        {
            _branches = _dbContext.BranchStore.ToList();

            if (Message != null)
            {
                SalesReport retrievedSalesReport = new SalesReport();
                List<Order> orders = new List<Order>();
                List<Customer> newCustomers = new List<Customer>();

                DateTime fromDate = DateTime.Parse(FromDate);
                DateTime toDate = DateTime.Parse(ToDate);
                if (AgentSelection != Convert.ToString(-1))
                {
                    orders = _dbContext.Order.Where(
                    order => order.CreationLocationId.ToString() == AgentSelection
                    && fromDate <= order.CreationDate && order.CreationDate <= toDate).ToList();
                } else
                {
                    orders = _dbContext.Order.Where(order => fromDate <= order.CreationDate && order.CreationDate <= toDate).ToList();
                }
                
                newCustomers = _dbContext.Customer.Where(
                    cust => fromDate <= cust.CreationDate && cust.CreationDate <= toDate).ToList();

                retrievedSalesReport = CreateSalesReport(orders);
                retrievedSalesReport.TypeOfTimeline = $"From {fromDate.ToString("M")} to {toDate.ToString("M")}";
                retrievedSalesReport.TotalCustomers = newCustomers.Count;

                // Get revenue statistics
                DateTime curDate = fromDate;
                List<RevenueItem> revenues = new List<RevenueItem>();
                while (fromDate <= curDate && curDate <= toDate) 
                {
                    Decimal totalReveueOfDate;
                    if (AgentSelection != Convert.ToString(-1))
                    {
                        totalReveueOfDate = _dbContext.Order
                            .Where(order => order.CreationLocationId.ToString() == AgentSelection && order.CreationDate.Date == curDate.Date)
                            .Sum(order => order.ActualBill);
                    } else
                    {
                        totalReveueOfDate = _dbContext.Order
                            .Where(order => order.CreationDate.Date == curDate.Date)
                            .Sum(order => order.ActualBill);
                    }
                    
                    revenues.Add(new RevenueItem() { Revenue = totalReveueOfDate, DateVal = curDate });
                    curDate = curDate.AddDays(1);
                }
                retrievedSalesReport.RevenueItems = revenues;
                _res = new ResponseStatus(Status, Message, retrievedSalesReport);
            }
        }

        public IActionResult OnPostViewSalesReport(string AgentSelection, string FromDate, string ToDate)
        {
            if (AgentSelection != null)
            {
                _res = new ResponseStatus(true, "Start retrieve products.", AgentSelection, FromDate, ToDate);
                return RedirectToPage("/HeadQuarter/RevenueStatistics", _res);
            }

            _res = new ResponseStatus(false, "The process is suspend.");
            return RedirectToPage("/HeadQuarter/RevenueStatistics", _res);
        }

        private SalesReport CreateSalesReport(List<Order> orders)
        {
            SalesReport salesReport = new SalesReport();
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            Decimal importPrices = 0;

            Decimal totalAmount = 0;
            long numOrProducts = 0;
            Decimal totalProdit = 0;
            foreach (Order order in orders)
            {
                totalAmount += order.ActualBill;
                orderDetails = _dbContext.OrderDetail.Where(od => od.OrderId == order.Id).ToList();
                numOrProducts += orderDetails.Count;

                Product? productInOrder;
                foreach (OrderDetail orderDetail in orderDetails)
                {
                    productInOrder = _dbContext.Product.Where(p => p.Id == orderDetail.ProductId).FirstOrDefault();
                    if (productInOrder != null)
                    {
                        importPrices += productInOrder.ImportPrice;
                    }
                }
                totalProdit = totalAmount - importPrices;
            }

            salesReport.TotalAmount = totalAmount;
            salesReport.TotalOrders = orders.Count;
            salesReport.TotalProducts = numOrProducts;
            salesReport.TotalProfit = totalProdit;
            salesReport.Orders = orders;
            return salesReport;
        }
    }
}