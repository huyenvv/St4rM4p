using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class EventApiController : BaseApiController
    {
        private readonly StarMapEntities _db;

        public EventApiController()
        {
            _db = new StarMapEntities();
        }

        // GET api/EventApi
        public object GetEvent(string lang, int page, int cateId = 0)
        {
            List<Event> data;
            if (cateId > 0)
            {
                data = _db.Event.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId && m.IsActive).ToList();
            }
            else
            {
                data = _db.Event.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.IsActive).ToList();
            }
            var count = data.Count;
            var start = Common.GetPaging(page, PageSize, count);
            return new
            {
                totalPage = ((count - 1) / PageSize) + 1,
                data = count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToEventModel()).ToList() : new List<EventModel>()
            };
        }

        // GET api/EventApi
        public object GetEvent(string location, string lang, int page, int cateId = 0)
        {
            List<Event> lst;
            if (cateId > 0)
            {
                lst = _db.Event.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId && m.IsActive).ToList();
            }
            else
            {
                lst = _db.Event.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.IsActive).ToList();
            }
            var data = new List<Event>();
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
                data = count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToEventModel()).ToList() : new List<EventModel>()
            };
        }

        // GET api/EventApi/5
        [ResponseType(typeof(EventModel))]
        public IHttpActionResult GetEvent(int id)
        {
            Event even = _db.Event.FirstOrDefault(m=>m.Id==id && m.IsActive);
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