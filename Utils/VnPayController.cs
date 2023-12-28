using POSWebsite.Models;

namespace POSWebsite.Utils
{
    public class VnPayController : IVnPayController
    { 
        private readonly IConfiguration _configuration;

        public VnPayController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(Payment model, HttpContext context)
        {
            TimeZoneInfo timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            DateTime timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            VnPayLibrary pay = new VnPayLibrary();
            string urlCallBack = _configuration["VNPAYSettings:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["VNPAYSettings:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["VNPAYSettings:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["VNPAYSettings:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((long)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["VNPAYSettings:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["VNPAYSettings:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.OrderId}");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

            string paymentUrl =
                pay.CreateRequestUrl(_configuration["VNPAYSettings:BaseUrl"], _configuration["VNPAYSettings:HashSecret"]);

            return paymentUrl;
        }

        public PaymentResponse PaymentExecute(IQueryCollection collections)
        {
            VnPayLibrary pay = new VnPayLibrary();
            PaymentResponse response = pay.GetFullResponseData(collections, _configuration["VNPAYSettings:HashSecret"]);

            return response;
        }
    }
}
