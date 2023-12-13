using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace POSWebsite.Controllers
{
    public class OnlyUnauthenticatedAttribute : TypeFilterAttribute
    {
        public OnlyUnauthenticatedAttribute() : base(typeof(AccountTypeAttributeFilter))
        {
        }

        private class AccountTypeAttributeFilter : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
        }
    }
}
