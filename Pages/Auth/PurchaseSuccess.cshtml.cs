using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;
using POSWebsite.Utils;

namespace POSWebsite.Pages.Auth
{
    public class PurchaseSuccessModel : PageModel
    {
        private readonly ILogger<PurchaseSuccessModel> _logger;
        private readonly B2BDbContrext _dbContext;
        private int _orderId;

        public PurchaseSuccessModel(ILogger<PurchaseSuccessModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void OnGet(int orderId)
        {
            _orderId = orderId;
        }

        public int GetOrderId()
        {
            return _orderId;
        }

        public ActionResult OnGetPrintReport(string orderid)
        {
            Order? order = _dbContext.Order.Where(o => o.Id.ToString() == orderid).FirstOrDefault();
            List<OrderDetail> orderDetail = _dbContext.OrderDetail.Where(od => od.OrderId.ToString() == orderid).ToList();

            PDFGenerator generator = new PDFGenerator();
            return generator.CreatePDF(order, orderDetail);
        }
    }
}
