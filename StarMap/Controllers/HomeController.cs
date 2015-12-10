using System;
using System.Web;
using System.Web.Mvc;
using StarMap.Helpers;

namespace StarMap.Controllers
{
    public class HomeController : BaseController
    {
        [CustomAuthorize]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Category");
        }

        public ActionResult SetCulture(string culture, string returnUrl = "")
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);

            // Save culture in a cookie
            var cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {

                cookie = new HttpCookie("_culture")
                {
                    Value = culture,
                    Expires = DateTime.Now.AddYears(1)
                };
            }
            Response.Cookies.Add(cookie);

            returnUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : Url.Action("Index");
            return Redirect(returnUrl);
        }

        public ActionResult ShowAllApi()
        {
            return View();
        }
    }
}
