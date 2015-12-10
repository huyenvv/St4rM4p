using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Configuration;
using Newtonsoft.Json.Linq;
using StarMap.Models;

namespace StarMap.Utilities
{
    public class Common
    {
        public static string RouteData(string key)
        {
            var obj = HttpContext.Current.Request.RequestContext.RouteData.Values[key];
            return obj == null ? null : obj.ToString();
        }
        private static readonly string[] VietnameseSigns = new string[]
        {
                        "aAeEoOuUiIdDyY",                        
                        "áàạảãâấầậẩẫăắằặẳẵ",                        
                        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",                        
                        "éèẹẻẽêếềệểễ",                        
                        "ÉÈẸẺẼÊẾỀỆỂỄ",                        
                        "óòọỏõôốồộổỗơớờợởỡ",                        
                        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",                        
                        "úùụủũưứừựửữ",                        
                        "ÚÙỤỦŨƯỨỪỰỬỮ",                        
                        "íìịỉĩ",                        
                        "ÍÌỊỈĨ",                        
                        "đ",                        
                        "Đ",                        
                        "ýỳỵỷỹ",                        
                        "ÝỲỴỶỸ"
        };

        public static string RemoveSign4Vietnamese(string str)
        {

            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {

                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;
        }

        public static string RemoveSymbol(string str)
        {
            //Giữ 1 ký tự trắng
            str = string.Join(" ", str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            //Loại trừ các ký tự đặc biệt - ngoài các ký tự sau
            string notSymbol =
                        " -[]abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" +
                        "áàạảãâấầậẩẫăắằặẳẵ" +
                        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ" +
                        "éèẹẻẽêếềệểễ" +
                        "ÉÈẸẺẼÊẾỀỆỂỄ" +
                        "óòọỏõôốồộổỗơớờợởỡ" +
                        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ" +
                        "úùụủũưứừựửữ" +
                        "ÚÙỤỦŨƯỨỪỰỬỮ" +
                        "íìịỉĩ" +
                        "ÍÌỊỈĨ" +
                        "đ" +
                        "Đ" +
                        "ýỳỵỷỹ" +
                        "ÝỲỴỶỸ";

            for (int i = 0; i < str.Length; i++)
            {
                if (notSymbol.IndexOf(str[i]) == -1)
                {
                    str = str.Remove(i, 1);
                    i--;
                }
            }
            return str;
        }

        public static string CreateURLParam(int id, string value)
        {
            return string.Format("{0}-{1}", RemoveSymbol(RemoveSign4Vietnamese(value)).Replace(" ", "-").Replace("--", "-"), id);
        }
        public static string CreateURLParam(string value)
        {
            return RemoveSymbol(RemoveSign4Vietnamese(value)).Replace(" ", "-").Replace("--", "-");
        }
        public static int GetIDFromURLParam(string param)
        {
            //Trả về 0 nếu không lọc được ID. Khi gọi, kiểm tra != 0 thì thực hiện tiếp
            int id = 0;
            if (!string.IsNullOrEmpty(param))
            {
                int startIndex = param.LastIndexOf('-');
                int endIndex = param.Length;

                if (startIndex < endIndex)
                {
                    int.TryParse(param.Substring(startIndex + 1, endIndex - startIndex - 1), out id);
                }
            }
            return id;
        }

        public static string getYesNO(bool value)
        {
            if (value)
            {
                return "YES";
            }
            return "NO";
        }
        public static DateTime? ChangeFormatDate(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }
            string[] t = date.Trim().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            return new DateTime(int.Parse(t[2]), int.Parse(t[0]), int.Parse(t[1]));
        }
        public static string BuildDropdown(object[] values, object[] texts, object selectedValue, string none = "- none -")
        {
            System.Text.StringBuilder st = new StringBuilder();
            if (none != null)
                st = new StringBuilder("<option value=''>" + none + "</option>");
            string s = selectedValue + "";
            for (int i = 0; i < values.Length; i++)
            {
                if (s.Equals(values[i]))
                    st.Append(string.Format("<option value='{0}' selected='selected'>{1}</option>", values[i], texts[i]));
                else
                    st.Append(string.Format("<option value='{0}'>{1}</option>", values[i], texts[i]));
            }

            return st.ToString();
        }
        public static string BuildDropdown(int[] values, object[] texts, object selectedValue, string none = "- none -")
        {
            System.Text.StringBuilder st = new StringBuilder();
            if (none != null)
                st = new StringBuilder("<option value=''>" + none + "</option>");
            string s = selectedValue + "";
            for (int i = 0; i < values.Length; i++)
            {
                if (s.Equals(values[i]))
                    st.Append(string.Format("<option value='{0}' selected='selected'>{1}</option>", values[i], texts[i]));
                else
                    st.Append(string.Format("<option value='{0}'>{1}</option>", values[i], texts[i]));
            }

            return st.ToString();
        }

        public static async Task<JObject> GetRequestResponse(string url)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri(url);
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                return (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<JObject> PostRequestResponse(string url, FormUrlEncodedContent data)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri(url);
                var response = await client.PostAsync(uri, data);
                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                return (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    public static class Cultures
    {
        public static readonly CultureInfo UnitedKingdom =
            CultureInfo.ReadOnly(new CultureInfo("en-US"));
    }
    
    public static class Extensions
    {
        public static string AppSetting(this string key)
        {
            string ret = string.Empty;

            if (ConfigurationManager.AppSettings[key] != null)
            {
                ret = ConfigurationManager.AppSettings[key];
            }

            return ret;
        }

        public static GoldPointModel ToGoldPointModel(this GoldPoint model)
        {
            return new GoldPointModel
            {
                Id = model.Id,
                Address = model.Address,
                DetailDescription = model.DetailDescription,
                DetailImage = model.DetailImage,
                IsHot = model.IsHot,
                Location = model.Location,
                Mobile = model.Mobile,
                Name = model.Name,
                ThumbDescription = model.ThumbDescription,
                ThumbImage = model.ThumbImage
            };
        }

        public static EventModel ToEventModel(this Event model)
        {
            return new EventModel
            {
                Id = model.Id,
                Address = model.Address,
                DetailDescription = model.DetailDescription,
                DetailImage = model.DetailImage,
                IsHot = model.IsHot,
                Location = model.Location,
                Mobile = model.Mobile,
                Name = model.Name,
                ThumbDescription = model.ThumbDescription,
                ThumbImage = model.ThumbImage
            };
        }

        public static SaleModel ToSaleModel(this Sale model)
        {
            return new SaleModel
            {
                Id = model.Id,
                Address = model.Address,
                DetailDescription = model.DetailDescription,
                DetailImage = model.DetailImage,
                Rate = model.Rate,
                Location = model.Location,
                Mobile = model.Mobile,
                Name = model.Name,
                ThumbDescription = model.ThumbDescription,
                ThumbImage = model.ThumbImage
            };
        }
    }

}