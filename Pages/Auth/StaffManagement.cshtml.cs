using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;
using POSWebsite.Utils;
using System.Security.Claims;
using System.Web.Helpers;

namespace POSWebsite.Pages.Auth
{
    public class StaffManagementModel : PageModel
    {
        private readonly ILogger<StaffManagementModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private IWebHostEnvironment _environment;
        private Staff _curStaff;
        private BranchStore _curBranchStore;
        private List<StaffInfoHolder> _staffListAtStore;

        public StaffManagementModel(ILogger<StaffManagementModel> logger, B2BDbContrext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = webHostEnvironment;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public Staff GetCurStaff()
        {
            return _curStaff;
        }

        public BranchStore GetCurBranchStore()
        {
            return _curBranchStore;
        }

        public List<StaffInfoHolder> GetStaffListAtStore()
        {
            return _staffListAtStore;
        }

        public void OnGet(bool Status, string Message)
        {
            _staffListAtStore = new List<StaffInfoHolder>();
            if (Message != null)
            {
                _res = new ResponseStatus(Status, Message);
            }

            var claims = User.FindFirst(claim => claim.Type == ClaimTypes.Email);
            if (claims != null)
            {
                Staff? staff = _dbContext.Staff.Where(s => s.Email == claims.Value).FirstOrDefault();
                if (staff != null)
                {
                    BranchStore? branchStore = _dbContext.BranchStore.Where(branch => branch.Id == staff.BranchStoreId).FirstOrDefault();
                    if (branchStore != null)
                    {
                        _curBranchStore = branchStore;
                    }

                    _curStaff = staff;
                    List<Staff> staffList = _dbContext.Staff.Where(s => s.BranchStoreId == staff.BranchStoreId).ToList();
                    foreach (Staff staffAtStore in staffList)
                    {
                        Account? account = _dbContext.Account.Where(s => s.Email == staffAtStore.Email).FirstOrDefault();
                        _staffListAtStore.Add(
                            new StaffInfoHolder() { StaffInfo = staffAtStore, AccountInfo = account });
                    }
                }
            }
        }

        public async Task<IActionResult> OnGetLockOrUnlockStaffAccount(string email)
        {
            Account? account = await _dbContext.Account.FirstAsync(s => s.Email == email);
            if (account != null)
            {
                account.IsLocked = !account.IsLocked;
                await _dbContext.SaveChangesAsync();

                _res = new ResponseStatus(true, "Account's condition is modified successfully.");
                return RedirectToPage("/Auth/StaffManagement", _res);
            }

            _res = new ResponseStatus(false, "There is an error when we'r trying to toggle this account's condition.");
            return RedirectToPage("/Auth/StaffManagement", _res);
        }

        public async Task<IActionResult>  OnGetResendActivation(string email)
        {
            Account? account = await _dbContext.Account.FirstAsync(s => s.Email == email);
            if (account != null)
            {
                string activationToken = Guid.NewGuid().ToString();
                account.ActivationToken = activationToken;
                account.TokenCreatedAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();

                EmailSender emailSender = new EmailSender();
                string baseUrl = string.Format("{0}://{1}", Request.Scheme, Request.Host);
                string link = $"{baseUrl}/Activation?token={activationToken}";
                string subject = "[Action required] Activate your account";
                string body = $"Please click on the this <a href='{link}'>link</a> to activate your account.";
                await emailSender.SendEmailAsync(email, subject, body);

                _res = new ResponseStatus(true, $"A activation email is sent to mailbox of {email} successfully");
                return RedirectToPage("/Auth/StaffManagement", _res);
            }

            _res = new ResponseStatus(false, "There is an error when we'r trying to send activation email.");
            return RedirectToPage("/Auth/StaffManagement", _res);
        }
    }
}
