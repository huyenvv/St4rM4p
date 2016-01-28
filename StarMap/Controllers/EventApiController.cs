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
        public object GetEvent(string lang, int page, int cateId = 0, string searchText = "", string location = "")
        {
            IEnumerable<Event> lst = cateId > 0
                ? _db.Event.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId && m.IsActive)
                : _db.Event.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.IsActive);

            if (!string.IsNullOrEmpty(searchText))
                lst = lst.Where(m => m.Name.ToLower().Contains(searchText.ToLower()));

            var data = new List<Event>();
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
                data = count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToEventModel()).ToList() : new List<EventModel>()
            };
        }

        // GET api/EventApi/5
        [ResponseType(typeof(EventModel))]
        public IHttpActionResult GetEvent(int id)
        {
            Event even = _db.Event.FirstOrDefault(m => m.Id == id && m.IsActive);
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