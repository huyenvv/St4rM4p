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
        public List<GoldPointModel> GetGoldPoint(string lang, int page, int cateId=0)
        {
            List<GoldPoint> data;
            if (cateId> 0)
            {
                data = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId).ToList();
            }
            else
            {
                data = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower()).ToList();
            }
            var count = data.Count;
            var start = Common.GetPaging(page, PageSize, count);
            return count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToGoldPointModel()).ToList() : new List<GoldPointModel>();
        }

        // GET api/GoldPointApi
        public List<GoldPointModel> GetGoldPoint(string location, string lang, int page, int cateId=0)
        {
            List<GoldPoint> lst;
            if (cateId > 0)
            {
                lst = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId).ToList();
            }
            else
            {
                lst = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower()).ToList();
            }
            var data = new List<GoldPoint>();
            foreach (var item in lst)
            {
                var distance = GoogleHelpers.DistanceTwoLocation(location, item.Location);
                if (distance <= DistanceAround)
                {
                    data.Add(item);
                }
            }
            var count = data.Count;
            var start = Common.GetPaging(page, PageSize, count);
            return count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToGoldPointModel()).ToList() : new List<GoldPointModel>();
        }

        // GET api/GoldPointApi/5
        [ResponseType(typeof(GoldPointModel))]
        public IHttpActionResult GetGoldPoint(int id)
        {
            GoldPoint goldpoint = _db.GoldPoint.Find(id);
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