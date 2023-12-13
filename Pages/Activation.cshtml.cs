using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;

namespace POSWebsite.Pages
{
    public class ActivationPageModel : PageModel
    {
        private readonly ILogger<ActivationPageModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;

        public ActivationPageModel(ILogger<ActivationPageModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public async Task<IActionResult> OnGet(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _res = new ResponseStatus(false, "Cannot find your account's credentials. Please, contact manager to get support.");
                return RedirectToPage("/Error", _res);
            }
            try
            {
                Account? account = _dbContext.Account.Where(account => account.ActivationToken == token).FirstOrDefault();
                if (account == null)
                {
                    _res = new ResponseStatus(false, "Cannot find your account's credentials. Please, contact manager to get support.");
                    return RedirectToPage("/Error", _res);
                }

                if (DateTime.Now.Subtract((DateTime) account.TokenCreatedAt).TotalMinutes > 1)
                {
                    _res = new ResponseStatus(false, "Your activation link expired. Please, contact manager to get support.");
                    return RedirectToPage("/Error", _res);
                }

                if (account.IsActivated)
                {
                    _res = new ResponseStatus(true, "This account've already activated. Please log in to continue.");
                    return RedirectToPage("/Index", _res);
                }

                account.ActivationToken = null;
                account.IsActivated = true;
                await _dbContext.SaveChangesAsync();

                _res = new ResponseStatus(true, "This account is activated successfully. Please log in to continue.");
                return RedirectToPage("/Index", _res);
            }
            catch (Exception ex)
            {
                _res = new ResponseStatus(false, "Something went wrong.");
                return RedirectToPage("/Error", _res);
            }
        }
    }
}
