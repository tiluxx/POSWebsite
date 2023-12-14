using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;
using POSWebsite.Utils;
using System.Data;
using System.Reflection;

namespace POSWebsite.Pages.Auth
{
    public class CreateStaffAccountModel : PageModel
    {
        private readonly ILogger<CreateStaffAccountModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;

        public CreateStaffAccountModel(ILogger<CreateStaffAccountModel> logger, B2BDbContrext dbContext)
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

        public async Task<IActionResult> OnPost(string Fullname, string Email, string TelNo, string BirthDate, string Gender, string[] Roles) 
        {
            if (!ModelState.IsValid)
            {
                _res = new ResponseStatus(false, "You've not entered all required information.");
                return RedirectToPage("/Auth/CreateStaffAccount", _res);
            }

            // Create staff
            string defaultMalePicture = "https://firebasestorage.googleapis.com/v0/b/b2b-solution-app.appspot.com/o/profile_pictures%2Fdefault_male.jpg?alt=media&token=29505c75-c857-467a-863c-06faf65183d5";
            string defaultFemalePicture = "https://firebasestorage.googleapis.com/v0/b/b2b-solution-app.appspot.com/o/profile_pictures%2Fdefault_female.jpg?alt=media&token=f28e1736-5216-41de-ab2d-e6a3eb16ed19";

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
                IsWorking = true,
                ProfilePictureUrl = Gender == "Male" ? defaultMalePicture : defaultFemalePicture
            };

            bool isExisted = await _dbContext.Account.AnyAsync(staff => staff.Email == Email);
            /*if (isExisted)
            {
                _res = new ResponseStatus(false, "This staff is existed in the system. Please check again.");
                return RedirectToPage("/Auth/CreateStaffAccount", _res);
            }*/

            await _dbContext.Staff.AddAsync(newStaff);
            await _dbContext.SaveChangesAsync();

            // Create account for staff
            newStaff = _dbContext.Staff.Where(s => s.Email == Email).FirstOrDefault();
            if (newStaff == null)
            {
                _res = new ResponseStatus(false, "There is error when we're trying to create new staff.");
                return RedirectToPage("/Auth/CreateStaffAccount", _res);
            }

            string activationToken = Guid.NewGuid().ToString();
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
                IsNewPasswordCreated = false,
                IsLocked = false,
            };
            await _dbContext.Account.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            EmailSender emailSender = new EmailSender();
            string baseUrl = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            string link = $"{baseUrl}/Activation?token={activationToken}";
            string subject = "[Action required] Activate your account";
            string body = $"Please click on the this <a href='{link}'>link</a> to activate your account.";
            await emailSender.SendEmailAsync(Email, subject, body);

            _res = new ResponseStatus(true, $"A activation email is sent to mailbox of saleperson - {Fullname} successfully");
            return RedirectToPage("/Auth/CreateStaffAccount", _res);
        }
    }
}
