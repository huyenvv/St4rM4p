using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarMap.Utilities
{
    public class FileUpload
    {
        public const string ROOT = "/Upload";
        #region Files
        public static string GetExtension(string fileName, out string baseName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                baseName = null;
                return null;
            }
            int k = fileName.LastIndexOf('.');
            if (k > 0)
            {
                baseName = fileName.Substring(0, k);
                return fileName.Substring(k, fileName.Length - k);
            }
            else
            {
                baseName = fileName;
                return null;
            }
        }
        public static string CreateFileName(string fileName, string folder)
        {
            string basename = null;
            string extension = GetExtension(fileName, out basename);
            fileName = Common.CreateURLParam(basename) + extension;
            return fileName;
        }
        public static string CreateFullName(string fileName, string folder)
        {
            string basename = null;
            string extension = GetExtension(fileName, out basename);
            fileName = Common.CreateURLParam(basename) + extension;
            if (folder != null && folder[0] != '/') folder = '/' + folder;
            if (folder != null && folder[folder.Length - 1] != '/') folder += '/';
            fileName = ROOT + folder + fileName;
            return fileName;
        }

        public static bool FileExist(string fullName)
        {
            return System.IO.File.Exists(HttpContext.Current.Server.MapPath(fullName));
        }

        public static string ReadString(string fullName, bool addRoot = true)
        {
            fullName = HttpContext.Current.Server.MapPath(fullName);
            using (System.IO.StreamReader filestream = new System.IO.StreamReader(fullName))
            {
                return filestream.ReadToEnd();
            }
        }

        public static void CreateFile(HttpPostedFileBase file, out string fullName, DateTime? date = null)
        {
            date = date == null ? DateTime.Now : date;
            CreateDirectory(date.Value.Year.ToString());
            string monthDir = date.Value.Year + "/" + date.Value.Month;
            CreateDirectory(monthDir);
            CreateFile(file, monthDir, out fullName);
        }

        public static void CreateFile(HttpPostedFileBase file, out string fullName, bool overrideExists, DateTime? date = null)
        {
            if (overrideExists)
            {
                CreateFile(file, out fullName, date);
                return;
            }

            date = date == null ? DateTime.Now : date;
            CreateDirectory(date.Value.Year.ToString(), true);
            string monthDir = date.Value.Year + "/" + date.Value.Month;
            CreateDirectory(monthDir, true);

            string fileName = file.FileName;
            fullName = CreateFullName(fileName, monthDir);
            while (FileExist(fullName))
            {
                fileName = DateTime.Now.Millisecond + "-" + fileName;
                fullName = CreateFullName(fileName, monthDir);
            }

            file.SaveAs(HttpContext.Current.Server.MapPath(fullName));
        }

        public static void CreateFile(HttpPostedFileBase file, string folder, out string fullName)
        {
            string fileName = file.FileName;
            fileName = CreateFullName(fileName, folder);
            fullName = fileName;
            file.SaveAs(HttpContext.Current.Server.MapPath(fullName));
        }

        public static void CreateFile(HttpPostedFileBase file, string folder)
        {
            string fileName = file.FileName;
            fileName = CreateFullName(fileName, folder);
            file.SaveAs(HttpContext.Current.Server.MapPath(fileName));
        }

        /// <summary>
        /// Tạo file đồng thời tạo thư mục con dạng Năm/Tháng/FileName lấy từ biến date. fileName sẽ được xử lý về dạng chuẩn. Nếu có bất kì file nào tồn tại với tên file sau khi được xử lý, hệ thống sẽ tự động thêm tiền tố vào tên file cho đến khi không trùng với bất kì tên file nào trong cùng folder.
        /// </summary>
        public static void CreateFile(byte[] data, string fileName, out string fullName, DateTime? date = null)
        {
            CreateFile(data, fileName, out fullName, false);
        }

        /// <summary>
        /// Tạo file đồng thời tạo thư mục con dạng Năm/Tháng/FileName lấy từ biến date. fileName sẽ được xử lý về dạng chuẩn. Nếu overrideExist = false và có bất kì file nào tồn tại với tên file sau khi được xử lý, hệ thống sẽ tự động thêm tiền tố vào tên file cho đến khi không trùng với bất kì tên file nào trong cùng folder, ngược lại thì file bị ghi đè
        /// </summary>
        public static void CreateFile(byte[] data, string fileName, out string fullName, bool overrideExist, DateTime? date = null)
        {
            date = date == null ? DateTime.Now : date;
            CreateDirectory(date.Value.Year.ToString());
            string monthDir = date.Value.Year + "/" + date.Value.Month;
            CreateDirectory(monthDir);
            fullName = CreateFullName(fileName, monthDir);
            while (!overrideExist && FileExist(fullName))
            {
                fileName = DateTime.Now.Millisecond + "-" + fileName;
                fullName = CreateFullName(fileName, monthDir);
            }
            CreateFile(data, fullName);
        }

        public static void CreateFile(byte[] data, string fullName)
        {
            using (System.IO.FileStream filestream = new System.IO.FileStream(HttpContext.Current.Server.MapPath(fullName), System.IO.FileMode.Create))
            {
                filestream.Write(data, 0, data.Length);
            }
        }

        public static void RemoveFile(string fullName)
        {
            if (FileExist(fullName))
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(fullName));
            }
        }
        #endregion

        #region Directories
        public static bool DirectoryExist(string dir, bool appendRoot = true)
        {
            if (appendRoot)
            {
                if (dir != null && dir[0] != '/') dir = '/' + dir;
                return System.IO.Directory.Exists(ROOT + dir);
            }
            return System.IO.Directory.Exists(dir);
        }
        public static void CreateDirectory(string dir, bool appendRoot = true)
        {
            if (appendRoot)
            {
                if (!DirectoryExist(dir, appendRoot))
                {
                    if (dir != null && dir[0] != '/') dir = '/' + dir;
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ROOT + dir));
                    return;
                }
            }
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dir));
        }
        #endregion
    }
}