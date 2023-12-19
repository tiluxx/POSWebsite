using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSWebsite.Models;
using System.IO;
using System.Security.Claims;
using System.Web.Helpers;

namespace POSWebsite.Pages.Auth
{
    public class MyProfileModel : PageModel
    {
        private readonly ILogger<MyProfileModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private IWebHostEnvironment _environment;
        private Staff _curStaff;
        private BranchStore _curBranchStore;

        public MyProfileModel(ILogger<MyProfileModel> logger, B2BDbContrext dbContext, IWebHostEnvironment webHostEnvironment)
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

        public void OnGet(bool Status, string Message)
        {
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
                    _curStaff = staff;
                    BranchStore? branch = _dbContext.BranchStore.Where(b => b.Id == staff.BranchStoreId).FirstOrDefault();
                    if (branch != null)
                    {
                        _curBranchStore = branch;
                    }
                }
            }
        }

        public async Task<IActionResult> OnPost(IFormFile profilePicture, string curStaffEmail)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                try
                {
                    var file = Path.Combine(_environment.ContentRootPath, profilePicture.FileName);
                    var fileStream = new FileStream(file, FileMode.Create);
                    await profilePicture.CopyToAsync(fileStream);
                    fileStream.Close();

                    var stream = System.IO.File.Open(file, FileMode.Open);
                    string newFileName = $"{Guid.NewGuid().ToString().Replace("-", "")}_{Path.GetFileName(profilePicture.FileName)}";
                    var task = new FirebaseStorage("b2b-solution-app.appspot.com")
                        .Child("profile_pictures")
                        .Child(newFileName)
                        .PutAsync(stream);
                    string? downloadUrl = await task;
                    stream.Close();
                    System.IO.File.Delete(file);

                    Staff? staff = _dbContext.Staff.Where(s => s.Email == curStaffEmail).FirstOrDefault();
                    if (staff != null)
                    {
                        staff.ProfilePictureUrl = downloadUrl;
                        await _dbContext.SaveChangesAsync();
                    }

                    _res = new ResponseStatus(true, "New profile picture is updated");
                    return RedirectToPage("/Auth/MyProfile", _res);
                } catch (Exception ex)
                {
                    _res = new ResponseStatus(false, "There is an error when we're trying to update new picture.");
                    return RedirectToPage("/Auth/MyProfile", _res);
                }
            }
            _res = new ResponseStatus(false, "There is an error when we're trying to update new picture.");
            return RedirectToPage("/Auth/MyProfile", _res);
        }
    }
}
