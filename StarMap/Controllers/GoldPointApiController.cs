using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class GoldPointApiController : BaseApiController
    {
        private readonly StarMapEntities _db = new StarMapEntities();
        
        // GET api/GoldPointApi
        public object GetGoldPoint(string lang, int page, int cateId = 0, string searchText = "", string location = "")
        {
            IEnumerable<GoldPoint> lst = cateId > 0 
                ? _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId && m.IsActive).ToList() 
                : _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.IsActive).ToList();
            if (!string.IsNullOrEmpty(searchText))
                lst = lst.Where(m => m.Name.ToLower().Contains(searchText.ToLower()));

            var data = new List<GoldPoint>();
            if (!string.IsNullOrEmpty(location))
            {
                foreach (var item in lst)
                {
                    var distance = GoogleHelpers.DistanceTwoLocation(location, item.Location);
                    if (distance <= RadiusSearch)
                    {
                        data.Add(item);
                    }
                }
            }
            else
            {
                data = lst.ToList();
            }
            var count = data.Count;
            var start = Common.GetPaging(page, PageSize, count);
            return new
            {
                totalPage = ((count - 1) / PageSize) + 1,
                data = count > 0 
                    ? data.Skip(start).OrderByDescending(m=>m.Rate).Take(PageSize).Select(m => m.ToGoldPointModel()).ToList() 
                    : new List<GoldPointModel>()
            };
        }

        // GET api/GoldPointApi/5
        [ResponseType(typeof(GoldPointModel))]
        public IHttpActionResult GetGoldPoint(int id)
        {
            GoldPoint goldpoint = _db.GoldPoint.FirstOrDefault(m => m.Id == id && m.IsActive);
            if (goldpoint == null)
            {
                return NotFound();
            }

            return Ok(goldpoint.ToGoldPointModel());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}