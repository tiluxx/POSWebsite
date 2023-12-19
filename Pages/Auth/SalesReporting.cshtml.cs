using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using POSWebsite.Models;
using System;
using System.Web.WebPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace POSWebsite.Pages.Auth
{
    public class SalesReportingModel : PageModel
    {
        private readonly ILogger<SalesReportingModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;

        public SalesReportingModel(ILogger<SalesReportingModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public void OnGet(bool Status, string Message, string TimelineSelection, string QuarterSelection, string QuarterYearSelection, string YearSelection, string FromDate, string ToDate)
        {
            if (Message != null)
            {
                SalesReport retrievedSalesReport = new SalesReport();
                List<Order> orders = new List<Order>();
                List<Customer> newCustomers = new List<Customer>();
                switch (TimelineSelection)
                {
                    case "today":
                        DateTime today = DateTime.Now;
                        orders = _dbContext.Order.Where(
                            order => order.CreationDate.Day == today.Day
                            && order.CreationDate.Month == today.Month
                            && order.CreationDate.Year == today.Year).ToList();
                        newCustomers = _dbContext.Customer.Where(
                            cust => cust.CreationDate.Day == today.Day
                            && cust.CreationDate.Month == today.Month
                            && cust.CreationDate.Year == today.Year).ToList();

                        retrievedSalesReport = CreateSalesReport(orders);
                        retrievedSalesReport.TypeOfTimeline = "Today";
                        retrievedSalesReport.TotalCustomers = newCustomers.Count;
                        break;
                    case "yesterday":
                        DateTime yesterday = DateTime.Now.AddDays(-1);
                        orders = _dbContext.Order.Where(
                            order => order.CreationDate.Day == yesterday.Day
                            && order.CreationDate.Month == yesterday.Month
                            && order.CreationDate.Year == yesterday.Year).ToList();
                        newCustomers = _dbContext.Customer.Where(
                            cust => cust.CreationDate.Day == yesterday.Day
                            && cust.CreationDate.Month == yesterday.Month
                            && cust.CreationDate.Year == yesterday.Year).ToList();

                        retrievedSalesReport = CreateSalesReport(orders);
                        retrievedSalesReport.TypeOfTimeline = "Yesterday";
                        retrievedSalesReport.TotalCustomers = newCustomers.Count;
                        break;
                    case "last7Days":
                        DateTime endDate = DateTime.Now;
                        DateTime startDate = DateTime.Now.AddDays(-6);
                        orders = _dbContext.Order.Where(
                            order => startDate <= order.CreationDate && order.CreationDate <= endDate).ToList();
                        newCustomers = _dbContext.Customer.Where(
                            cust => startDate <= cust.CreationDate && cust.CreationDate <= endDate).ToList();

                        retrievedSalesReport = CreateSalesReport(orders);
                        retrievedSalesReport.TypeOfTimeline = "Last 7 days";
                        retrievedSalesReport.TotalCustomers = newCustomers.Count;
                        _res = new ResponseStatus(true, "Report retrieved.", retrievedSalesReport);
                        break;
                    case "thisMonth":
                        int thisMonth = DateTime.Now.Month;
                        orders = _dbContext.Order.Where(order => order.CreationDate.Month == thisMonth).ToList();
                        newCustomers = _dbContext.Customer.Where(order => order.CreationDate.Month == thisMonth).ToList();

                        retrievedSalesReport = CreateSalesReport(orders);
                        retrievedSalesReport.TypeOfTimeline = "This month";
                        retrievedSalesReport.TotalCustomers = newCustomers.Count;
                        break;
                    case "fromDateToDate":
                        DateTime fromDate = DateTime.Parse(FromDate);
                        DateTime toDate = DateTime.Parse(ToDate);
                        orders = _dbContext.Order.Where(
                            order => fromDate <= order.CreationDate && order.CreationDate <= toDate).ToList();
                        newCustomers = _dbContext.Customer.Where(
                            cust => fromDate <= cust.CreationDate && cust.CreationDate <= toDate).ToList();

                        retrievedSalesReport = CreateSalesReport(orders);
                        retrievedSalesReport.TypeOfTimeline = $"From {fromDate.ToString("M")} to {toDate.ToString("M")}";
                        retrievedSalesReport.TotalCustomers = newCustomers.Count;
                        break;
                    case "quarter":
                        int quarterSelection = Convert.ToInt32(QuarterSelection);
                        int quarterYearSelection = Convert.ToInt32(QuarterYearSelection);
                        DateTime firstDayOfQuarter = GetFirstDayOfQuarter(quarterYearSelection, quarterSelection);
                        DateTime lastDayOfQuarter = GetLastDayOfQuarter(firstDayOfQuarter);
                        orders = _dbContext.Order.Where(
                            order => firstDayOfQuarter <= order.CreationDate && order.CreationDate <= lastDayOfQuarter).ToList();
                        newCustomers = _dbContext.Customer.Where(
                            cust => firstDayOfQuarter <= cust.CreationDate && cust.CreationDate <= lastDayOfQuarter).ToList();

                        retrievedSalesReport = CreateSalesReport(orders);
                        retrievedSalesReport.TypeOfTimeline = $"In Q{quarterSelection}, {quarterYearSelection}";
                        retrievedSalesReport.TotalCustomers = newCustomers.Count;
                        break;
                    case "year":
                        orders = _dbContext.Order.Where(order => order.CreationDate.Year == Convert.ToInt32(YearSelection)).ToList();
                        newCustomers = _dbContext.Customer.Where(cust => cust.CreationDate.Year == Convert.ToInt32(YearSelection)).ToList();
                        retrievedSalesReport = CreateSalesReport(orders);
                        retrievedSalesReport.TypeOfTimeline = $"In {YearSelection}";
                        retrievedSalesReport.TotalCustomers = newCustomers.Count;
                        break;
                    default:
                        break;
                }
                _res = new ResponseStatus(Status, Message, retrievedSalesReport);
            }
        }

        public IActionResult OnPostViewSalesReport(string TimelineSelection, string QuarterSelection, string QuarterYearSelection, string YearSelection, string FromDate, string ToDate)
        {
            if (TimelineSelection != null)
            {
                _res = new ResponseStatus(true, "Start retrieve products.", TimelineSelection, QuarterSelection, QuarterYearSelection, YearSelection, FromDate, ToDate);
                return RedirectToPage("/Auth/SalesReporting", _res);
            }

            _res = new ResponseStatus(false, "The process is suspend.");
            return RedirectToPage("/Auth/SalesReporting", _res);
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
                orderDetails = _dbContext.OrderDetail.Where(od => od.Order.Id == order.Id).ToList();
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

        private DateTime GetFirstDayOfQuarter(int year, int quarter)
        {
            int month = (quarter - 1) * 3 + 1;
            return new DateTime(year, month, 1);
        }

        private DateTime GetLastDayOfQuarter(DateTime firstDayOfQuarter)
        {
            return firstDayOfQuarter.AddMonths(3).AddDays(-1);
        }
    }
}
