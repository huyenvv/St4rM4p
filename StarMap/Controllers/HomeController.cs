using System;
using System.Web;
using System.Web.Mvc;
using StarMap.Helpers;
using StarMap.Models;
using System.Linq;

namespace StarMap.Controllers
{
    public class HomeController : BaseController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        [CustomAuthorize]
        public ActionResult Index()
        {
            var cates = _db.Category.Where(m => m.Lang == CurrentLang);
            var goldPoint = _db.GoldPoint.Where(m => m.Lang == CurrentLang);
            var sale = _db.Sale.Where(m => m.Lang == CurrentLang);
            var eventList = _db.Event.Where(m => m.Lang == CurrentLang);

            var dashboard = new DashboardModel();
            dashboard.GPCount = goldPoint.Count();
            dashboard.GPActiveCount = goldPoint.Where(m => m.IsActive).Count();
            dashboard.GPInActiveCount = dashboard.GPCount - dashboard.GPActiveCount;
            dashboard.GPCategoriesList = cates.AsEnumerable().Select(m => new GeneralObject
            {
                Key = m.Name,
                Value = string.Format("{0}", m.GoldPoint.Count)
            }).ToList();
            dashboard.GPRatingList = goldPoint.GroupBy(m => m.Rate).AsEnumerable().Select(m => new GeneralObject
            {
                Key = m.Key,
                Value = string.Format("{0}", m.Count())
            }).ToList();

            dashboard.EventCount = eventList.Count();
            dashboard.EventActiveCount = eventList.Where(m => m.IsActive).Count();
            dashboard.EventInActiveCount = dashboard.EventCount - dashboard.EventActiveCount;
            dashboard.EventCategoriesList = cates.AsEnumerable().Select(m => new GeneralObject
            {
                Key = m.Name,
                Value = string.Format("{0}", m.Event.Count)
            }).ToList();

            dashboard.SaleCount = sale.Count();
            dashboard.SaleActiveCount = sale.Where(m => m.IsActive).Count();
            dashboard.SaleInActiveCount = dashboard.SaleCount - dashboard.SaleActiveCount;
            dashboard.SaleHotCount = sale.Where(m => m.IsHot.HasValue && m.IsHot.Value).Count();
            //dashboard.SaleNewCount = 0;
            dashboard.SaleCategoriesList = cates.AsEnumerable().Select(m => new GeneralObject
            {
                Key = m.Name,
                Value = string.Format("{0}", m.Sale.Count)
            }).ToList();

            return View(dashboard);
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
