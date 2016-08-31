using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StarMap.Models;
using StarMap.Helpers;
using System.Threading.Tasks;
using StarMap.Utilities;
using Microsoft.AspNet.Identity;
using PagedList;

namespace StarMap.Controllers
{
    [CustomAuthorize]
    public class EventController : BaseController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET: /Event/
        public ActionResult Index(int? page, int? cateId, bool? status, bool? isHot, DateTime? startDate, DateTime? endDate, string searchText = "")
        {
            if (!page.HasValue || page.Value < 1)
            {
                page = 1;
            }
            var list = _db.Event.Where(m => m.Lang == CurrentLang);
            if (cateId.HasValue)
            {
                list = list.Where(m => m.CategoryId == cateId);
            }
            if (status.HasValue)
            {
                list = list.Where(m => m.IsActive == status.Value);
            }

            if (isHot.HasValue)
            {
                list = list.Where(m => m.IsHot == isHot.Value);
            }

            if (startDate.HasValue)
            {
                list = list.Where(m => m.StartDate >= startDate);
            }
            if (endDate.HasValue)
            {
                list = list.Where(m => m.EndDate <= endDate);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                list = list.Where(m => (!string.IsNullOrEmpty(m.Name) && m.Name.ToLower().Contains(searchText))
                    || (!string.IsNullOrEmpty(m.Address) && m.Address.ToLower().Contains(searchText)));
            }
            ViewBag.categoryList = new SelectList(_db.Category.Where(m => m.Lang == CurrentLang), "Id", "Name", cateId);
            ViewBag.status = status;
            ViewBag.searchText = searchText;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            ViewBag.isHot = isHot;
            return View(list.OrderBy(m => m.Name).ToPagedList(page.Value, PageSize));
        }

        // GET: /Event/Edit/5
        public async Task<ActionResult> NewOrEdit(int id = 0)
        {
            ViewBag.Category = _db.Category.Where(m => m.Lang == CurrentLang).ToList();
            if (id == 0)
            {
                return View(new EventModel());
            }
            Event even = await _db.Event.FindAsync(id);
            if (even == null)
            {
                return View(new EventModel());
            }
            if (even.Lang != CurrentLang)
            {
                return RedirectToAction("Index");
            }
            return View(even.ToEventModel());
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewOrEdit([Bind(Include = "Id,Name,Address,Mobile,Location,ThumbImage,DetailImage,ThumbDescription,DetailDescription,CategoryId,StartDate,EndDate,IsHot,IsActive,Country,City")] 
            EventModel even, HttpPostedFileBase thumbImagePathFile, HttpPostedFileBase detailImagePathFile)
        {
            if (ModelState.IsValid)
            {

                var newEvent = _db.Event.FirstOrDefault(m => m.Id == even.Id);
                if (newEvent == null) newEvent = new Event
                {
                    CreateDate = DateTime.Now,
                    CreatedBy = User.Identity.GetUserId(),
                    Lang = CultureHelper.GetCurrentCulture(true)
                };

                newEvent.Name = even.Name;
                newEvent.Address = even.Address;
                newEvent.Mobile = even.Mobile;
                newEvent.Location = even.Location;
                newEvent.ThumbDescription = even.ThumbDescription;
                newEvent.DetailDescription = even.DetailDescription;
                newEvent.IsHot = even.IsHot;
                newEvent.CategoryId = even.CategoryId;
                newEvent.StartDate = even.StartDate;
                newEvent.EndDate = even.EndDate;
                newEvent.IsActive = even.IsActive;
                newEvent.Country = even.Country;
                newEvent.City = even.City;
                if (thumbImagePathFile != null)
                {
                    string fileName = null;
                    FileUpload.CreateFile(thumbImagePathFile, out fileName, false);
                    newEvent.ThumbImage = fileName;
                }
                if (detailImagePathFile != null)
                {
                    string fileName = null;
                    FileUpload.CreateFile(detailImagePathFile, out fileName, false);
                    newEvent.DetailImage = fileName;
                }

                if (even.Id == 0)
                {
                    _db.Event.Add(newEvent);
                }
                else
                {
                    _db.Entry(newEvent).State = EntityState.Modified;
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Category = _db.Category.Where(m => m.Lang == CurrentLang).ToList();
            return View(even);
        }

        // GET: /Event/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event even = await _db.Event.FindAsync(id);
            if (even == null)
            {
                return HttpNotFound();
            }

            var detailImage = even.DetailImage;
            var thumbImage = even.ThumbImage;

            _db.Event.Remove(even);
            await _db.SaveChangesAsync();

            // Remove Image
            FileUpload.RemoveFile(detailImage);
            FileUpload.RemoveFile(thumbImage);

            return RedirectToAction("Index");
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
