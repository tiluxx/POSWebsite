using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;
using POSWebsite.Pages.Auth;
using POSWebsite.Utils;
using System.Security.Claims;

namespace POSWebsite.Pages.HeadQuarter
{
    public class ManagerManagementModel : PageModel
    {
        private readonly ILogger<ManagerManagementModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private IWebHostEnvironment _environment;
        private Staff _curStaff;
        private List<StaffInfoHolder> _staffListAtStore;

        public ManagerManagementModel(ILogger<ManagerManagementModel> logger, B2BDbContrext dbContext, IWebHostEnvironment webHostEnvironment)
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
                    List<BranchStore> branchStore = _dbContext.BranchStore.Select(branch => branch).ToList();
                    List<Staff> staffList = new List<Staff>();
                    foreach (BranchStore branch in branchStore)
                    {
                        staffList = _dbContext.Staff.Where(s => s.BranchStoreId == branch.Id).ToList();
                        foreach (Staff staffAtStore in staffList)
                        {
                            Account? account = _dbContext.Account.Where(s => s.Email == staffAtStore.Email).FirstOrDefault();
                            if (account != null && account.Roles.Contains("Manager"))
                            {
                                _staffListAtStore.Add(
                                new StaffInfoHolder() { StaffInfo = staffAtStore, AccountInfo = account, BranchStoreInfo = branch });
                            }
                        }
                    }
                    _curStaff = staff;
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
                return RedirectToPage("/HeadQuarter/ManagerManagement", _res);
            }

            _res = new ResponseStatus(false, "There is an error when we'r trying to toggle this account's condition.");
            return RedirectToPage("/HeadQuarter/ManagerManagement", _res);
        }

        public async Task<IActionResult> OnGetResendActivation(string email)
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
                return RedirectToPage("/HeadQuarter/ManagerManagement", _res);
            }

            _res = new ResponseStatus(false, "There is an error when we'r trying to send activation email.");
            return RedirectToPage("/HeadQuarter/ManagerManagement", _res);
        }
    }
}
