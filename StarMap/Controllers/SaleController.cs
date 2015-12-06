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
    public class SaleController : BaseController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET: /Sale/
        public async Task<ActionResult> Index()
        {
            var lang = CultureHelper.GetCurrentCulture(true);
            return View(await _db.Sale.Where(m => m.Lang == lang).ToListAsync());
        }

        // GET: /Sale/Edit/5
        public async Task<ActionResult> NewOrEdit(int id = 0)
        {
            ViewBag.Category = _db.Category.ToList();
            if (id == 0)
            {
                return View(new Sale());
            }
            Sale Sale = await _db.Sale.FindAsync(id);
            if (Sale == null)
            {
                return View(new Sale());
            }
            return View(Sale);
        }

        // POST: /Sale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewOrEdit([Bind(Include = "Id,Name,Address,Mobile,Location,ThumbImage,DetailImage,ThumbDescription,DetailDescription,Rate,CategoryId")] 
            Sale Sale, HttpPostedFileBase thumbImagePathFile, HttpPostedFileBase detailImagePathFile)
        {
            if (ModelState.IsValid)
            {

                var newSale = _db.Sale.FirstOrDefault(m => m.Id == Sale.Id);
                if (newSale == null) newSale = new Sale
                {
                    CreateDate = DateTime.Now,
                    CreatedBy = User.Identity.GetUserId(),
                    Lang = CultureHelper.GetCurrentCulture(true)
                };

                newSale.Name = Sale.Name;
                newSale.Address = Sale.Address;
                newSale.Mobile = Sale.Mobile;
                newSale.Location = Sale.Location;
                newSale.ThumbDescription = Sale.ThumbDescription;
                newSale.DetailDescription = Sale.DetailDescription;
                newSale.Rate = Sale.Rate;
                newSale.CategoryId = Sale.CategoryId;
                if (thumbImagePathFile != null)
                {
                    string fileName = null;
                    FileUpload.CreateFile(thumbImagePathFile, out fileName, false);
                    newSale.ThumbImage = fileName;
                }
                if (detailImagePathFile != null)
                {
                    string fileName = null;
                    FileUpload.CreateFile(detailImagePathFile, out fileName, false);
                    newSale.DetailImage = fileName;
                }

                if (Sale.Id == 0)
                {
                    _db.Sale.Add(newSale);
                }
                else
                {
                    _db.Entry(newSale).State = EntityState.Modified;
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Category = _db.Category.ToList();
            return View(Sale);
        }

        // GET: /Sale/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale Sale = await _db.Sale.FindAsync(id);
            if (Sale == null)
            {
                return HttpNotFound();
            }
           
            var detailImage = Sale.DetailImage;
            var thumbImage = Sale.ThumbImage;          

            _db.Sale.Remove(Sale);
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
