using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.Security.Claims;

namespace POSWebsite.Pages.Auth
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;

        public IndexModel(ILogger<IndexModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public IActionResult OnGet()
        {
            var claims = User.FindFirst(claim => claim.Type == ClaimTypes.Email);
            if (claims != null)
            {
                Account? account = _dbContext.Account.Where(s => s.Email == claims.Value).FirstOrDefault();
                if (account != null && !account.IsNewPasswordCreated)
                {
                    _res = new ResponseStatus(false, "Force to create new password", claims.Value);
                    return RedirectToPage("/Auth/IndexFirstLogin", _res);
                }

                return Page();
            }
            return RedirectToPage("/Error");
        }
    }
}
