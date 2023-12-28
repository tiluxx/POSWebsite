using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop.Infrastructure;
using POSWebsite.Models;
using System;
using System.Security.Claims;

namespace POSWebsite.Pages.Auth
{
    public class AddProductModel : PageModel
    {
        private readonly ILogger<AddProductModel> _logger;
        private readonly B2BDbContrext _dbContext;
        private IWebHostEnvironment _environment;
        private BranchStore _curBranchStore;

        [BindProperty]
        public Product Product { get; set; }

        public AddProductModel(ILogger<AddProductModel> logger, B2BDbContrext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = webHostEnvironment;
        }

        public void OnGet()
        {
        }

        public BranchStore GetCurBranchStore()
        {
            return _curBranchStore;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile photo)
        {
            try
            {
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

                        if (photo != null && photo.Length > 0)
                        {
                            try
                            {
                                var file = Path.Combine(_environment.ContentRootPath, photo.FileName);
                                var fileStream = new FileStream(file, FileMode.Create);
                                await photo.CopyToAsync(fileStream);
                                fileStream.Close();

                                var stream = System.IO.File.Open(file, FileMode.Open);
                                string newFileName = $"{Guid.NewGuid().ToString().Replace("-", "")}_{Path.GetFileName(photo.FileName)}";
                                var task = new FirebaseStorage("b2b-solution-app.appspot.com")
                                    .Child("product_media")
                                    .Child(newFileName)
                                    .PutAsync(stream);
                                string? downloadUrl = await task;
                                stream.Close();
                                System.IO.File.Delete(file);

                                if (downloadUrl != null)
                                {
                                    Product.Photo = downloadUrl;

                                }
                                else
                                {
                                    Product.Photo = "https://firebasestorage.googleapis.com/v0/b/b2b-solution-app.appspot.com/o/profile_pictures%2Fabillion-Nf5fSqHm-iY-unsplash.jpg?alt=media&token=1fd71760-8fe1-4cb6-80b9-ad228692ca72";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            Product.Photo = "https://firebasestorage.googleapis.com/v0/b/b2b-solution-app.appspot.com/o/profile_pictures%2Fabillion-Nf5fSqHm-iY-unsplash.jpg?alt=media&token=1fd71760-8fe1-4cb6-80b9-ad228692ca72";
                        }

                        Product.CreationDate = DateTime.Now;

                        _dbContext.Product.Add(Product);
                        await _dbContext.SaveChangesAsync();

                        var updateProducts = _dbContext.Product.ToListAsync();
                        return RedirectToPage("/Auth/ListProduct", new { products = updateProducts });
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }

    }
}
