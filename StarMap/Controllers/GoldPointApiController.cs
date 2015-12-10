using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class GoldPointApiController : ApiController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET api/GoldPointApi
        public IQueryable<GoldPointModel> GetGoldPoint()
        {
            return _db.GoldPoint.Select(m => m.ToGoldPointModel());
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