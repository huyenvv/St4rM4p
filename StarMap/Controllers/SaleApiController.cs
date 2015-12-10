using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class SaleApiController : ApiController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET api/SaleApi
        public IQueryable<SaleModel> GetSale()
        {
            return _db.Sale.Select(m => m.ToSaleModel());
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