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
    public class SaleController : BaseController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET: /Sale/
        public ActionResult Index(int? page, int? cateId, bool? status, bool? isHot, DateTime? startDate, DateTime? endDate, string searchText = "")
        {
            if (!page.HasValue || page.Value < 1)
            {
                page = 1;
            }
            var list = _db.Sale.Where(m => m.Lang == CurrentLang);
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

        // GET: /Sale/Edit/5
        public async Task<ActionResult> NewOrEdit(int id = 0)
        {
            ViewBag.Category = _db.Category.Where(m => m.Lang == CurrentLang).ToList();
            if (id == 0)
            {
                return View(new SaleModel());
            }
            Sale sale = await _db.Sale.FindAsync(id);
            if (sale == null)
            {
                return View(new SaleModel());
            }
            if (sale.Lang != CurrentLang)
            {
                return RedirectToAction("Index");
            }
            return View(sale.ToSaleModel());
        }

        // POST: /Sale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewOrEdit([Bind(Include = "Id,Name,Address,Mobile,Location,ThumbImage,DetailImage,ThumbDescription,DetailDescription,IsHot,CategoryId,StartDate,EndDate,IsActive,Country,City")] 
            SaleModel Sale, HttpPostedFileBase thumbImagePathFile, HttpPostedFileBase detailImagePathFile)
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
                newSale.IsHot = Sale.IsHot;
                newSale.CategoryId = Sale.CategoryId;
                newSale.StartDate = Sale.StartDate;
                newSale.EndDate = Sale.EndDate;
                newSale.IsActive = Sale.IsActive;
                newSale.Country = Sale.Country;
                newSale.City = Sale.City;

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
            ViewBag.Category = _db.Category.Where(m => m.Lang == CurrentLang).ToList();
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
