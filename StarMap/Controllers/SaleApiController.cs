using System;
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
        public object GetSale(string lang, int page, int cateId = 0, string searchText = "", string location = "")
        {
            IEnumerable<Sale> lst = cateId > 0
                ? _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && String.Equals(m.Lang, lang, StringComparison.CurrentCultureIgnoreCase) && m.CategoryId == cateId && m.IsActive).ToList()
                : _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && String.Equals(m.Lang, lang, StringComparison.CurrentCultureIgnoreCase) && m.IsActive).ToList();
            if (!string.IsNullOrEmpty(searchText))
                lst = lst.Where(m => m.Name.ToLower().Contains(searchText.ToLower()));

            var data = new List<Sale>();
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