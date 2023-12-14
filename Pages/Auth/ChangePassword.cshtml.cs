using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace POSWebsite.Pages.Auth
{
    public class ChangePasswordModel : PageModel
    {
        private readonly ILogger<ChangePasswordModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private string _curUserEmail;

        public ChangePasswordModel(ILogger<ChangePasswordModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public string GetCurUserEmail()
        {
            return _curUserEmail;
        }

        public void OnGet(bool Status, string Message)
        {
            var claims = User.FindFirst(claim => claim.Type == ClaimTypes.Email);
            if (claims != null)
            {
                _curUserEmail = claims.Value;
            }

            if (Message != null)
            {
                _res = new ResponseStatus(Status, Message);
            }
        }

        public async Task<IActionResult> OnPost(string OldPassword, string NewPassword, string ConfirmNewPassword, string Email)
        {
            if (!ModelState.IsValid)
            {
                _res = new ResponseStatus(false, "Please enter new password to log in.");
                return RedirectToPage("/Auth/ChangePassword", _res);
            }

            Account? account = _dbContext.Account.Where(user => user.Email == Email & user.Password == OldPassword).FirstOrDefault();
            if (account == null)
            {
                _res = new ResponseStatus(false, "Your old password is not matched.");
                return RedirectToPage("/Auth/ChangePassword", _res);
            }

            if (NewPassword != ConfirmNewPassword)
            {
                _res = new ResponseStatus(false, "Password does not matched.");
                return RedirectToPage("/Auth/ChangePassword", _res);
            }

            account.Password = NewPassword;
            await _dbContext.SaveChangesAsync();

            _res = new ResponseStatus(true, "New password is updated successfully.");
            return RedirectToPage("/Auth/ChangePassword", _res);
        }
    }
}
