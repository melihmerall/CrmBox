using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CrmBox.WebUI.Controllers
{
    public class CultureController : Controller
    {
        [HttpPost]
        public IActionResult CultureManagement(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1)});
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
