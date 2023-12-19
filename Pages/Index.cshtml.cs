using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;
using System.Security.Claims;

namespace POSWebsite.Pages
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

        public void OnGet(bool Status, string Message)
        {
            if (Message != null)
            {
                _res = new ResponseStatus(Status, Message);
            }
        }

        public async Task<IActionResult> OnPost(string username, string password)
        {
            if (!ModelState.IsValid)
            {
                _res = new ResponseStatus(false, "Please enter all required information to log in.");
                return RedirectToPage("/Index", _res);
            }

            Account? account = _dbContext.Account.Where(user => user.Username == username && user.Password == password).FirstOrDefault();
            if (account != null)
            {
                if (!account.IsActivated)
                {
                    _res = new ResponseStatus(false, "Please login by clicking on the link in your email.");
                    return RedirectToPage("/Index", _res);
                }

                if (account.Roles.Contains("Admin"))
                {
                    await SaveLogin(account);
                    return Redirect("/HeadQuarter/Index");
                }
                else
                {
                    await SaveLogin(account);
                    return Redirect("/Auth/Index");
                }
            }

            _res = new ResponseStatus(false, "Account not found.");
            return RedirectToPage("/Index", _res);
        }

        private async Task SaveLogin(Account account)
        {
            string role = "User";
            if (account.Roles.Contains("Manager"))
            {
                role = "Manager";
            }

            if (account.Roles.Contains("Admin"))
            {
                role = "Admin";
            }

            string profilePictureUrl;
            Staff? staff = _dbContext.Staff.Where(s => s.Email == account.Email).FirstOrDefault();
            if (staff != null)
            {
                profilePictureUrl = staff.ProfilePictureUrl;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Name, account.Fullname),
                new Claim(ClaimTypes.Role, role),
                new Claim("Avatar", account.Staff.ProfilePictureUrl)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);
        }
    }
}