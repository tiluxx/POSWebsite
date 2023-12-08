using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;

namespace POSWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ResponseStatus res;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public ResponseStatus GetResponseStatus()
        {
            return res;
        }

        public void OnGet()
        {

        }
    }
}