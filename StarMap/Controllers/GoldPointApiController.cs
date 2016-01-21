﻿using System.Collections.Generic;
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
        public object GetGoldPoint(string lang, int page, int cateId=0)
        {
            List<GoldPoint> data;
            if (cateId> 0)
            {
                data = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId && m.IsActive).ToList();
            }
            else
            {
                data = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.IsActive).ToList();
            }
            var count = data.Count;
            var start = Common.GetPaging(page, PageSize, count);
            return new
            {
                totalPage = ((count - 1) / PageSize) + 1,
                data = count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToGoldPointModel()).ToList() : new List<GoldPointModel>()
            };
        }

        // GET api/GoldPointApi
        public object GetGoldPoint(string location, string lang, int page, int cateId=0)
        {
            List<GoldPoint> lst;
            if (cateId > 0)
            {
                lst = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.CategoryId == cateId && m.IsActive).ToList();
            }
            else
            {
                lst = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.ToLower() == lang.ToLower() && m.IsActive).ToList();
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
            return new
            {
                totalPage = ((count - 1) / PageSize) + 1,
                data = count > 0 ? data.Skip(start).Take(PageSize).Select(m => m.ToGoldPointModel()).ToList() : new List<GoldPointModel>()
            };
        }

        // GET api/GoldPointApi/5
        [ResponseType(typeof(GoldPointModel))]
        public IHttpActionResult GetGoldPoint(int id)
        {
            GoldPoint goldpoint = _db.GoldPoint.FirstOrDefault(m => m.Id == id && m.IsActive);
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