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
using System.IO;
using Ionic.Zip;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;

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
                    List<Event> list = new List<Event>();


                    TextFieldParser parser = new TextFieldParser(pathcsvFile);

                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.SetDelimiters(",");

                    string[] columns;

                    var now = DateTime.Now;
                    var errors = new List<ImportErrorModel>();
                    var count = 0;

                    while (!parser.EndOfData)
                    {
                        count++;
                        columns = parser.ReadFields();
                        if (columns.Length != 16)
                        {
                            if (count > 1)
                                errors.Add(new ImportErrorModel { Line = 0, Message = "Template format is not supported!" });
                            continue;
                        }

                        var cateName = columns[9];
                        var category = _db.Category.FirstOrDefault(m => m.Name.ToLower() == cateName);
                        if (category == null)
                        {
                            if (count > 1)
                                errors.Add(new ImportErrorModel { Line = count, Message = "Category incorrect" });
                            continue;
                        }

                        var location = columns[3];
                        Regex regexIsLocation = new Regex(@"^(\-?\d+(\.\d+)?),\s*(\-?\d+(\.\d+)?)$");
                        if (!regexIsLocation.IsMatch(location))
                        {
                            if (count > 1)
                                errors.Add(new ImportErrorModel { Line = count, Message = "Location format incorrect" });
                            continue;
                        }

                        var mobile = columns[2];
                        Regex regexIsMobile = new Regex(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$");
                        if (!regexIsMobile.IsMatch(mobile))
                        {
                            mobile = string.Empty;
                        }

                        var isActive = columns[15] == "1" ? true : false;
                        var isHot = columns[8] == "1" ? true : false;

                        list.Add(new Event
                        {
                            Name = columns[0],
                            Address = columns[1],
                            Mobile = mobile,
                            Location = location,
                            ThumbImage = folderPathRelative + "/" + columns[4],
                            DetailImage = folderPathRelative + "/" + columns[5],
                            ThumbDescription = columns[6],
                            DetailDescription = columns[7],
                            IsHot = isHot,
                            CategoryId = category.Id,
                            Lang = columns[10],
                            StartDate = Common.ConvertToDate(columns[11]),
                            EndDate = Common.ConvertToDate(columns[12]),
                            City = columns[13],
                            Country = columns[14],
                            IsActive = isActive,
                            CreateDate = now,
                            CreatedBy = User.Identity.GetUserId()
                        });
                    }

                    parser.Close();

                    // Save to Database
                    _db.Event.AddRange(list);
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
                    ViewBag.ErrorRows = Common.BuildErrorImportMessage(errors);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileResult GetTemplate()
        {
            var filePath = Server.MapPath("~\\App_Data\\Template\\Event.csv");
            return File(filePath, "text/csv", "template_event.csv");
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
