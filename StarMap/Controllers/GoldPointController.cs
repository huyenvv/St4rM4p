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
using PagedList;

namespace StarMap.Controllers
{
    [CustomAuthorize]
    public class GoldPointController : BaseController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET: /GoldPoint/
        public ActionResult Index(int? page, int? cateId, bool? status, string rate = "", string searchText = "")
        {
            if (!page.HasValue || page.Value < 1)
            {
                page = 1;
            }
            var list = _db.GoldPoint.Where(m => m.Lang == CurrentLang);
            if (cateId.HasValue)
            {
                list = list.Where(m => m.CategoryId == cateId);
            }
            if (status.HasValue)
            {
                list = list.Where(m => m.IsActive == status.Value);
            }
            if (!string.IsNullOrEmpty(rate))
            {
                list = list.Where(m => m.Rate == rate);
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                list = list.Where(m => (!string.IsNullOrEmpty(m.Name) && m.Name.ToLower().Contains(searchText))
                    || (!string.IsNullOrEmpty(m.Address) && m.Address.ToLower().Contains(searchText)));
            }
            ViewBag.categoryList = new SelectList(_db.Category.Where(m => m.Lang == CurrentLang), "Id", "Name", cateId);
            ViewBag.status = status;
            ViewBag.rate = rate;
            ViewBag.searchText = searchText;
            return View(list.OrderBy(m => m.Name).ToPagedList(page.Value, PageSize));
        }

        // GET: /GoldPoint/Edit/5
        public async Task<ActionResult> NewOrEdit(int id = 0)
        {
            ViewBag.Category = _db.Category.Where(m => m.Lang == CurrentLang).ToList();
            if (id == 0)
            {
                return View(new GoldPointModel());
            }
            GoldPoint goldPoint = await _db.GoldPoint.FindAsync(id);
            if (goldPoint == null)
            {
                return View(new GoldPointModel());
            }
            if (goldPoint.Lang != CurrentLang)
            {
                return RedirectToAction("Index");
            }
            return View(goldPoint.ToGoldPointModel());
        }

        // POST: /GoldPoint/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewOrEdit([Bind(Include = "Id,Name,Address,Mobile,Location,ThumbImage,DetailImage,ThumbDescription,DetailDescription,Rate,CategoryId,IsActive,Country,City")] 
            GoldPointModel goldPoint, HttpPostedFileBase thumbImagePathFile, HttpPostedFileBase detailImagePathFile)
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
                newGoldPoint.Rate = goldPoint.Rate;
                newGoldPoint.CategoryId = goldPoint.CategoryId;
                newGoldPoint.IsActive = goldPoint.IsActive;
                newGoldPoint.Country = goldPoint.Country;
                newGoldPoint.City = goldPoint.City;
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
            ViewBag.Category = _db.Category.Where(m => m.Lang == CurrentLang).ToList();
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