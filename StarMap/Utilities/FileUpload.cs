using System;
using System.IO;
using System.Web;

namespace StarMap.Utilities
{
    public class FileUpload
    {
        public const string ROOT = "/images";
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
            byte[] binaryData;
            binaryData = new Byte[file.InputStream.Length];
            long bytesRead = file.InputStream.Read(binaryData, 0, (int)file.InputStream.Length);
            file.InputStream.Close();
            fullName = "data:image/png;base64," + System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
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

        public static string CreateFromBase64(string contentBase64)
        {
            var bytes = Convert.FromBase64String(contentBase64);
            var date = DateTime.Now;
            CreateDirectory(date.Year.ToString());
            string monthDir = date.Year + "/" + date.Month;
            CreateDirectory(monthDir);
            string design = monthDir + "/Design";
            CreateDirectory(design);
            string filePath = monthDir + string.Format("{0}_{1}_{2}_{3}.jpg", date.Year, date.Month, date.Minute, date.Second);
            filePath = CreateFullName(filePath, design);
            while (FileExist(filePath))
            {
                filePath = DateTime.Now.Millisecond + "-" + filePath;
            }

            using (var imageFile = new FileStream(HttpContext.Current.Server.MapPath(filePath), FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            return filePath;
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
                }
            }
            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dir));
        }
        #endregion
    }
}