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
        public object GetSale(string lang, int page, int cateId = 0)
        {
            List<Sale> data;
            if (cateId > 0)
            {
                data = _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId && m.IsActive).ToList();
            }
            else
            {
                data = _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.IsActive).ToList();
            }

            var count = data.Count;
            var start = Common.GetPaging(page, PageSize, count);
            return new
            {
                totalPage = ((count - 1) / PageSize) + 1,
                data = count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToSaleModel()).ToList() : new List<SaleModel>()
            };
        }

        // GET api/SaleApi
        public object GetSale(string location, string lang, int page, int cateId = 0)
        {
            List<Sale> lst;
            if (cateId > 0)
            {
                lst = _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId && m.IsActive).ToList();
            }
            else
            {
                lst = _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.IsActive).ToList();
            }
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
            return new
            {
                totalPage = ((count - 1) / PageSize) + 1,
                data = count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToSaleModel()).ToList() : new List<SaleModel>()
            };
        }

        // GET api/SaleApi/5
        [ResponseType(typeof(SaleModel))]
        public IHttpActionResult GetSale(int id)
        {
            Sale sale = _db.Sale.FirstOrDefault(m => m.Id == id && m.IsActive);
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