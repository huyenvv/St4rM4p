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
using PagedList;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using Microsoft.VisualBasic.FileIO;

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


        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase csvFile = null, HttpPostedFileBase imagesZipFiles = null)
        {
            if (csvFile.ContentLength > 0 && imagesZipFiles.ContentLength > 0)
            {
                string csvFileName = Path.GetFileName(csvFile.FileName);
                string pathcsvFile = Path.Combine(Server.MapPath("~/App_Data/"), csvFileName);

                string imagesZipFilesName = Path.GetFileName(imagesZipFiles.FileName);
                string pathimagesZipFile = Path.Combine(Server.MapPath("~/App_Data/"), imagesZipFilesName);

                // Create Folder For Unzip
                var folderPathRelative = FileUpload.ROOT + DateTime.Now.ToString("/yyyy/MM/ddHHmmss");
                string folderFileImagesPath = Server.MapPath(folderPathRelative);
                if (!Directory.Exists(folderFileImagesPath))
                {
                    Directory.CreateDirectory(folderFileImagesPath);
                }

                try
                {
                    csvFile.SaveAs(pathcsvFile);
                    imagesZipFiles.SaveAs(pathimagesZipFile);

                    // Read CSV
                    List<GoldPoint> list = new List<GoldPoint>();


                    TextFieldParser parser = new TextFieldParser(pathcsvFile);

                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.SetDelimiters(",");

                    string[] columns;

                    var now = DateTime.Now;
                    var errorLines = new List<int>();
                    var count = 0;

                    while (!parser.EndOfData)
                    {
                        count++;
                        columns = parser.ReadFields();
                        if (columns.Length != 14)
                        {
                            if (count > 1)
                                errorLines.Add(count);
                            continue;
                        }

                        var cateName = columns[9];
                        var category = _db.Category.FirstOrDefault(m => m.Name.ToLower() == cateName);
                        if (category == null)
                        {
                            if (count > 1)
                                errorLines.Add(count);
                            continue;
                        }

                        var isActive = columns[13] == "1" ? true : false;

                        list.Add(new GoldPoint
                        {
                            Name = columns[0],
                            Address = columns[1],
                            Mobile = columns[2],
                            Location = columns[3],
                            ThumbImage = folderPathRelative + "/" + columns[4],
                            DetailImage = folderPathRelative + "/" + columns[5],
                            ThumbDescription = columns[6],
                            DetailDescription = columns[7],
                            Rate = columns[8],
                            CategoryId = category.Id,
                            Lang = columns[10],
                            City = columns[11],
                            Country = columns[12],
                            IsActive = isActive,
                            CreateDate = now,
                            CreatedBy = User.Identity.GetUserId()

                        });
                    }

                    parser.Close();

                    // Save to Database
                    _db.GoldPoint.AddRange(list);
                    var rowsAffect = _db.SaveChanges();

                    // Unzip
                    using (ZipFile zip = ZipFile.Read(pathimagesZipFile))
                    {
                        foreach (ZipEntry e in zip)
                        {
                            e.Extract(folderFileImagesPath);
                        }
                    }

                    // delete zip
                    System.IO.File.Delete(pathimagesZipFile);

                    // Delete File
                    System.IO.File.Delete(pathcsvFile);

                    ViewBag.Success = rowsAffect.ToString();
                    ViewBag.ErrorRows = string.Join(",", errorLines);
                }
                catch (Exception ex)
                {
                    //Delete Folder Unzip
                    Directory.Delete(folderFileImagesPath);

                    ViewBag.Feedback = ex.Message;
                }
            }
            else
            {
                ViewBag.Feedback = "Please select a file";
            }
            return View();
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