using POSWebsite.Models;

namespace POSWebsite.Utils
{
    public interface IVnPayController
    {
        string CreatePaymentUrl(Payment model, HttpContext context);
        PaymentResponse PaymentExecute(IQueryCollection collections);
    }
}
