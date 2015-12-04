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
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult SetCulture(string culture)
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

            return RedirectToAction("Index");
        }
    }
}
