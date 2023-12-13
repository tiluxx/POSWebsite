using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using POSWebsite.Models;
using System.Security.Claims;
using POSWebsite.Utils;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System;

namespace POSWebsite.Controllers
{
    public class AccountController : Controller
    {
        private B2BDbContrext _dbContext;

        public AccountController(B2BDbContrext context)
        {
            _dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            if (!ModelState.IsValid)
            {
                TempData["Status"] = false;
                TempData["Message"] = "Please enter all required information to log in.";
                return RedirectToAction("Index", "Home");
            }

            Account? account = _dbContext.Account.Where(user => user.Username == Username && user.Password == Password).FirstOrDefault();
            if (account != null)
            {
                if (!account.IsActivated)
                {
                    TempData["Status"] = false;
                    TempData["Message"] = "Please login by clicking on the link in your email.";
                    return RedirectToAction("Index", "Home");
                }

                if (account.Roles.Contains("Admin"))
                {
                    await SaveLogin(account);
                    return RedirectToAction("Index", "Auth");
                }
                else
                {
                    await SaveLogin(account);
                    return RedirectToAction("Index", "Auth");
                }
            }

            TempData["Status"] = false;
            TempData["Message"] = "Account not found.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaffAccount(string Fullname, string Email, string TelNo, string BirthDate, string Gender, string[] Roles)
        {
            if (!ModelState.IsValid)
            {
                TempData["Status"] = false;
                TempData["Message"] = "You've not entered all required information.";
                return RedirectToAction("CreateStaffAccount", "Auth");
            }

            // Create staff
            Staff newStaff = new Staff()
            {
                Fullname = Fullname,
                Email = Email,
                DateJoined = DateTime.Now,
                BranchName = "South Ho Chi Minh City",
                YearOfWork = 0,
                BirthDate = Convert.ToDateTime(BirthDate),
                Gender = Gender,
                TelNo = TelNo,
                Title = "Saleperson",
                IsWorking = true
            };

            await _dbContext.Staff.AddAsync(newStaff);
            await _dbContext.SaveChangesAsync();

            // Create account for staff
            newStaff = _dbContext.Staff.Where(s => s.Email == Email).FirstOrDefault();
            if (newStaff == null)
            {
                TempData["Status"] = false;
                TempData["Message"] = "There is error when we're trying to create new staff.";
                return RedirectToAction("CreateStaffAccount", "Auth");
            }

            string activationToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            string newUsername = Email.Split("@")[0];
            Account account = new Account()
            {
                Fullname = Fullname,
                Email = Email,
                Username = newUsername,
                Password = newUsername,
                Roles = Roles.ToList(),
                Staff = newStaff,
                ActivationToken = activationToken,
                IsActivated = false,
                TokenCreatedAt = DateTime.Now,
            };
            await _dbContext.Account.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            EmailSender emailSender = new EmailSender();
            string baseUrl = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            string link = $"{baseUrl}/Account/Activate?token={activationToken}";
            string subject = "[Action required] Activate your account";
            string body = $"Please click on the this <a href='{link}'>link</a> to activate your account.";
            await emailSender.SendEmailAsync(Email, subject, body);

            TempData["Status"] = true;
            TempData["Message"] = $"A activation email is sent to mailbox of saleperson - {Fullname} successfully";
            return RedirectToAction("CreateStaffAccount", "Auth");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Activate(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Message"] = "Cannot find your account's credentials. Please, contact manager to get support.";
                return RedirectToAction("Error", "Home");
            }
            try
            {
                Account? account = _dbContext.Account.Where(account => account.ActivationToken == token).FirstOrDefault();
                if (account == null)
                {
                    TempData["Message"] = "Cannot find your account's credentials. Please, contact manager to get support.";
                    return RedirectToAction("Error", "Home");
                }

                if (DateTime.Now.Subtract((DateTime)account.TokenCreatedAt).TotalMinutes > 1)
                {
                    TempData["Message"] = "Your activation link expired. Please, contact manager to get support.";
                    return RedirectToAction("Error", "Home");
                }

                if (account.IsActivated)
                {
                    TempData["ActivationStatus"] = true;
                    TempData["Message"] = "This account've already activated. Please log in to continue.";
                    return RedirectToAction("Index", "Auth");
                }

                account.ActivationToken = null;
                account.IsActivated = true;
                await _dbContext.SaveChangesAsync();

                TempData["ActivationStatus"] = true;
                TempData["Message"] = "This account is activated successfully. Please log in to continue.";
                return RedirectToAction("Index", "Auth");
            } 
            catch (Exception ex)
            {
                TempData["ActivationStatus"] = false;
                TempData["Message"] = "Something went wrong.";
                return RedirectToAction("Index", "Auth");
            }
        }

        public IActionResult Forbidden()
        {
            return View();
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
