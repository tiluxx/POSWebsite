using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.Security.Claims;

namespace POSWebsite.Pages.Auth
{
    public class StaffDetailModel : PageModel
    {
        private readonly ILogger<StaffDetailModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private StaffInfoHolder _curStaff;

        public StaffDetailModel(ILogger<StaffDetailModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public StaffInfoHolder GetCurStaff()
        {
            return _curStaff;
        }

        public void OnGet(string email)
        {
            Staff? staff = _dbContext.Staff.Where(s => s.Email == email).FirstOrDefault();
            if (staff != null)
            {
                Account? account = _dbContext.Account.Where(a => a.Email == email).FirstOrDefault();
                BranchStore? branch = _dbContext.BranchStore.Where(b => b.Id == staff.BranchStoreId).FirstOrDefault();
                _curStaff = new StaffInfoHolder() { StaffInfo = staff, AccountInfo = account, BranchStoreInfo = branch };
            }
        }
    }
}
