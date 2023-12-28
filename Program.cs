using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using POSWebsite.Models;
using POSWebsite.Utils;

namespace POSWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<B2BDbContrext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.LoginPath = "/";
                    options.LogoutPath = "/LogoutPage";
                    options.AccessDeniedPath = "/Forbidden";
                });

            builder.Services.AddScoped<PDFGenerator>();
            builder.Services.AddScoped<IVnPayController, VnPayController>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddSession();

            var app = builder.Build();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}