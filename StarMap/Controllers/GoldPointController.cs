using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StarMap.Helpers;
using StarMap.Models;
using StarMap.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StarMap.Controllers
{
    [CustomAuthorize]
    public class GoldPointController : BaseController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET: /GoldPoint/
        public async Task<ActionResult> Index()
        {
            var lang = CultureHelper.GetCurrentCulture(true);
            return View(await _db.GoldPoint.Where(m => m.Lang == lang).ToListAsync());
        }

        // GET: /GoldPoint/Edit/5
        public async Task<ActionResult> NewOrEdit(int id = 0)
        {
            ViewBag.Category = _db.Category.ToList();
            if (id == 0)
            {
                return View(new GoldPoint());
            }
            GoldPoint goldPoint = await _db.GoldPoint.FindAsync(id);
            if (goldPoint == null)
            {
                return View(new GoldPoint());
            }
            return View(goldPoint);
        }

        // POST: /GoldPoint/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewOrEdit([Bind(Include = "Id,Name,Address,Mobile,Location,ThumbImage,DetailImage,ThumbDescription,DetailDescription,IsHot,CategoryId")] 
            GoldPoint goldPoint, HttpPostedFileBase thumbImagePathFile, HttpPostedFileBase detailImagePathFile)
        {
            if (ModelState.IsValid)
            {

                var newGoldPoint = _db.GoldPoint.FirstOrDefault(m => m.Id == goldPoint.Id);
                if (newGoldPoint == null) newGoldPoint = new GoldPoint
                {
                    CreateDate = DateTime.Now,
                    CreatedBy = User.Identity.GetUserId(),
                    Lang = CultureHelper.GetCurrentCulture(true)
                };

                newGoldPoint.Name = goldPoint.Name;
                newGoldPoint.Address = goldPoint.Address;
                newGoldPoint.Mobile = goldPoint.Mobile;
                newGoldPoint.Location = goldPoint.Location;
                newGoldPoint.ThumbDescription = goldPoint.ThumbDescription;
                newGoldPoint.DetailDescription = goldPoint.DetailDescription;
                newGoldPoint.IsHot = goldPoint.IsHot;
                newGoldPoint.CategoryId = goldPoint.CategoryId;
                if (thumbImagePathFile != null)
                {
                    string fileName = null;
                    FileUpload.CreateFile(thumbImagePathFile, out fileName, false);
                    newGoldPoint.ThumbImage = fileName;
                }
                if (detailImagePathFile != null)
                {
                    string fileName = null;
                    FileUpload.CreateFile(detailImagePathFile, out fileName, false);
                    newGoldPoint.DetailImage = fileName;
                }

                if (goldPoint.Id == 0)
                {
                    _db.GoldPoint.Add(newGoldPoint);                    
                }
                else
                {
                    _db.Entry(newGoldPoint).State = EntityState.Modified;
                }                
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Category = _db.Category.ToList();
            return View(goldPoint);
        }

        // GET: /GoldPoint/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoldPoint goldPoint = await _db.GoldPoint.FindAsync(id);
            if (goldPoint == null)
            {
                return HttpNotFound();
            }

            var detailImage = goldPoint.DetailImage;
            var thumbImage = goldPoint.ThumbImage;

            _db.GoldPoint.Remove(goldPoint);
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