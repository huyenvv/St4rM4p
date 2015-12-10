using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class EventApiController : ApiController
    {
        private readonly StarMapEntities _db;

        public EventApiController()
        {
            _db = new StarMapEntities();
        }

        // GET api/EventApi
        public IQueryable<EventModel> GetEvent()
        {
            return _db.Event.Select(m => m.ToEventModel());
        }

        // GET api/EventApi/5
        [ResponseType(typeof(EventModel))]
        public IHttpActionResult GetEvent(int id)
        {
            Event even = _db.Event.Find(id);
            if (even == null)
            {
                return NotFound();
            }

            return Ok(even.ToEventModel());
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