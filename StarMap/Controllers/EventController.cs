using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.AspNet.Identity.EntityFramework;

namespace StarMap.Controllers
{
    [CustomAuthorize]
    public class EventController : BaseController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET: /Event/
        public async Task<ActionResult> Index()
        {
            var lang = CultureHelper.GetCurrentCulture(true);
            return View(await _db.Event.Where(m => m.Lang == lang).ToListAsync());
        }

        // GET: /Event/Edit/5
        public async Task<ActionResult> NewOrEdit(int id = 0)
        {
            ViewBag.Category = _db.Category.ToList();
            if (id == 0)
            {
                return View(new Event());
            }
            Event even = await _db.Event.FindAsync(id);
            if (even == null)
            {
                return View(new Event());
            }
            return View(even);
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewOrEdit([Bind(Include = "Id,Name,Address,Mobile,Location,ThumbImage,DetailImage,ThumbDescription,DetailDescription,IsHot,CategoryId")] 
            Event even, HttpPostedFileBase thumbImagePathFile, HttpPostedFileBase detailImagePathFile)
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
            ViewBag.Category = _db.Category.ToList();
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
