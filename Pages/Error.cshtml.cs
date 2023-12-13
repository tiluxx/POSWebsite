using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.Diagnostics;

namespace POSWebsite.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;
        private ResponseStatus _res;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public void OnGet(bool Status, string Message)
        {
            if (Message != null)
            {
                _res = new ResponseStatus(Status, Message);
            }
        }
    }
}