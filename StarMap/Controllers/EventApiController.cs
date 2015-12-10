using System.Collections.Generic;
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
        public List<EventModel> GetEvent()
        {
            var data = _db.Event.ToList();
            return data.Count > 0 ? data.Select(m => m.ToEventModel()).ToList() : new List<EventModel>();
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