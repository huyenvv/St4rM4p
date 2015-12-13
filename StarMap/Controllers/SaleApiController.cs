using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class SaleApiController : BaseApiController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET api/SaleApi
        public List<SaleModel> GetSale(string lang, int page)
        {
            var data = _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower()).ToList();
            var count = data.Count;
            var start = Common.GetPaging(page, PageSize, count);
            return count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToSaleModel()).ToList() : new List<SaleModel>();
        }

        // GET api/SaleApi
        public List<SaleModel> GetSale(string location, string lang, int page)
        {
            var lst = _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower()).ToList();
            var data = new List<Sale>();
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
            return count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToSaleModel()).ToList() : new List<SaleModel>();
        }

        // GET api/SaleApi/5
        [ResponseType(typeof(SaleModel))]
        public IHttpActionResult GetSale(int id)
        {
            Sale sale = _db.Sale.Find(id);
            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale.ToSaleModel());
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