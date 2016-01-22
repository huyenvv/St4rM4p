using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using StarMap.Helpers;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    [CustomAuthorize]
    public class CategoryController : BaseController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET: /Category/
        public async Task<ActionResult> Index(int? page)
        {
            if (!page.HasValue || page.Value < 1)
            {
                page = 1;
            }
            return View(_db.Category.Where(m => m.Lang == CurrentLang).OrderBy(m => m.Name).ToPagedList(page.Value, PageSize));
        }

        // GET: /Category/Edit/5
        public async Task<ActionResult> NewOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new CategoryModel());
            }
            Category category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return View(new CategoryModel());
            }
            if (category.Lang != CurrentLang)
            {
                return RedirectToAction("Index");
            }
            return View(category.ToCategoryModel());
        }

        // POST: /Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewOrEdit([Bind(Include = "Id,Name, Image, Lang")] CategoryModel category, HttpPostedFileBase imagePathFile)
        {
            if (ModelState.IsValid)
            {
                // check exists
                var exitsName = _db.Category.FirstOrDefault(m => m.Lang == CurrentLang && m.Id != category.Id && m.Name.ToLower() == category.Name.Trim().ToLower());
                if (exitsName != null)
                {
                    ModelState.AddModelError("", Resources.Resources.CategoryNameExists);
                    return View(category);
                }
                var newCategory = _db.Category.FirstOrDefault(m => m.Id == category.Id);
                if (newCategory == null) newCategory = new Category();
                newCategory.Name = category.Name;
                newCategory.Lang = CurrentLang;
                if (imagePathFile != null)
                {
                    string fileName;
                    FileUpload.CreateFile(imagePathFile, out fileName, false);
                    newCategory.Image = fileName;
                }
                if (category.Id == 0)
                {
                    if (imagePathFile == null)
                    {
                        ModelState.AddModelError("Image", Resources.Resources.ThisFieldIsRequired);
                        return View(category);
                    }
                    _db.Category.Add(newCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                _db.Entry(newCategory).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: /Category/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var image = category.Image;

            _db.Category.Remove(category);
            await _db.SaveChangesAsync();
            // Remove Image
            FileUpload.RemoveFile(image);
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