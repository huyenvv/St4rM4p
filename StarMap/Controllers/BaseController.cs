using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using StarMap.Helpers;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class BaseController : Controller
    {
        public int PageSize
        {
            get
            {
                return Convert.ToInt32("PageSizeWeb".AppSetting());
            }
        }

        public string CurrentLang
        {
            get
            {
                return CultureHelper.GetCurrentCulture(true);
            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


            return base.BeginExecuteCore(callback, state);
        }


    }
}