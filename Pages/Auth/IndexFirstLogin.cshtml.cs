using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.Security.Claims;

namespace POSWebsite.Pages.Auth
{
    public class IndexFirstLoginModel : PageModel
    {
        private readonly ILogger<IndexFirstLoginModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private string _curUserEmail;

        public IndexFirstLoginModel(ILogger<IndexFirstLoginModel> logger, B2BDbContrext dbContext)
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

        public void OnGet(bool Status, string Message, string Data)
        {
            if (Data != null)
            {
                _curUserEmail = Data;
            }

            if (Message != null)
            {
                _res = new ResponseStatus(Status, Message);
            }
        }

        public async Task<IActionResult> OnPost(string Password, string ConfirmPassword, string Email)
        {
            if (!ModelState.IsValid)
            {
                _res = new ResponseStatus(false, "Please enter new password to log in.");
                return RedirectToPage("/IndexFirstLogin", _res);
            }

            if (Password != ConfirmPassword)
            {
                _res = new ResponseStatus(false, "Password does not matched.");
                return RedirectToPage("/IndexFirstLogin", _res);
            }

            Account? account = _dbContext.Account.Where(user => user.Email == Email).FirstOrDefault();
            if (account != null)
            {
                account.Password = Password;
                account.IsNewPasswordCreated = true;
                await _dbContext.SaveChangesAsync();

                await SaveLogin(account);
                return Redirect("/Auth/Index");
            }

            _res = new ResponseStatus(false, "Account not found.");
            return RedirectToPage("/IndexFirstLogin", _res);
        }

        private async Task SaveLogin(Account account)
        {
            string role = "User";
            if (account.Roles.Contains("Admin"))
            {
                role = "Admin";
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Name, account.Fullname),
                new Claim(ClaimTypes.Role, role),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);
        }
    }
}
