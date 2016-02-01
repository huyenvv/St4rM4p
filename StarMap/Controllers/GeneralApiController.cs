using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class GeneralApiController : BaseApiController
    {
        private readonly StarMapEntities _db;

        public GeneralApiController()
        {
            _db = new StarMapEntities();
        }

        // GET api/General/SearchAll
        [HttpGet]
        public object SearchAll(string lang, int page, string types = "", string cates = "", string searchText = "", string location = "")
        {
            var listType = new string[] { };
            var listCate = new string[] { };
            if (!string.IsNullOrEmpty(types))
            {
                listType = types.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                listType = listType.Select(m => m.ToLower()).ToArray();
            }
            if (!string.IsNullOrEmpty(cates))
            {
                listCate = cates.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }

            IEnumerable<Sale> lstPromotion = _db.Sale.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.Equals(lang, StringComparison.CurrentCultureIgnoreCase) && m.IsActive);
            IEnumerable<Event> lstEvent = _db.Event.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.Equals(lang, StringComparison.CurrentCultureIgnoreCase) && m.IsActive);
            IEnumerable<GoldPoint> lstGoldPoint = _db.GoldPoint.Where(m => !string.IsNullOrEmpty(m.Lang) && m.Lang.Equals(lang, StringComparison.CurrentCultureIgnoreCase) && m.IsActive);


            if (listCate.Length > 0)
            {
                lstEvent = lstEvent.Where(m => listCate.Contains(m.CategoryId.ToString(CultureInfo.InvariantCulture)));
                lstGoldPoint = lstGoldPoint.Where(m => listCate.Contains(m.CategoryId.ToString(CultureInfo.InvariantCulture)));
                lstPromotion = lstPromotion.Where(m => listCate.Contains(m.CategoryId.ToString(CultureInfo.InvariantCulture)));
            }

            searchText = Common.RemoveSign4Vietnamese(Common.RemoveSymbol(searchText));
            if (!string.IsNullOrEmpty(searchText))
            {
                lstEvent = lstEvent.Where(m => Common.RemoveSign4Vietnamese(Common.RemoveSymbol(m.Name.ToLower())).Contains(searchText.ToLower()));
                lstGoldPoint = lstGoldPoint.Where(m => Common.RemoveSign4Vietnamese(Common.RemoveSymbol(m.Name.ToLower())).Contains(searchText.ToLower()));
                lstPromotion = lstPromotion.Where(m => Common.RemoveSign4Vietnamese(Common.RemoveSymbol(m.Name.ToLower())).Contains(searchText.ToLower()));
            }

            var allItems = new List<SearchAllModel>();
            var dataEvent = new List<Event>();
            var dataGoldPoint = new List<GoldPoint>();
            var dataPromotion = new List<Sale>();
            if (!string.IsNullOrEmpty(location))
            {
                if (listType.Contains(ApiType.Event))
                {
                    dataEvent.AddRange(from item in lstEvent let distance = GoogleHelpers.DistanceTwoLocation(location, item.Location) where distance <= RadiusSearch select item);
                }
                if (listType.Contains(ApiType.GoldPoint))
                {
                    dataGoldPoint.AddRange(from item in lstGoldPoint let distance = GoogleHelpers.DistanceTwoLocation(location, item.Location) where distance <= RadiusSearch select item);
                }
                if (listType.Contains(ApiType.Promotion))
                {
                    dataPromotion.AddRange(from item in lstPromotion let distance = GoogleHelpers.DistanceTwoLocation(location, item.Location) where distance <= RadiusSearch select item);
                }

                allItems.AddRange(dataEvent.Select(m => new SearchAllModel
                {
                    ItemId = m.Id,
                    Address = m.Address,
                    CategoryType = ApiType.Event,
                    Title = m.Name
                }));
                allItems.AddRange(dataGoldPoint.Select(m => new SearchAllModel
                {
                    ItemId = m.Id,
                    Address = m.Address,
                    CategoryType = ApiType.GoldPoint,
                    Title = m.Name
                }));
                allItems.AddRange(dataPromotion.Select(m => new SearchAllModel
                {
                    ItemId = m.Id,
                    Address = m.Address,
                    CategoryType = ApiType.Promotion,
                    Title = m.Name
                }));
            }
            else
            {
                allItems.AddRange(lstEvent.Select(m => new SearchAllModel
                {
                    ItemId = m.Id,
                    Address = m.Address,
                    CategoryType = ApiType.Event,
                    Title = m.Name
                }));
                allItems.AddRange(lstGoldPoint.Select(m => new SearchAllModel
                {
                    ItemId = m.Id,
                    Address = m.Address,
                    CategoryType = ApiType.GoldPoint,
                    Title = m.Name
                }));
                allItems.AddRange(lstPromotion.Select(m => new SearchAllModel
                {
                    ItemId = m.Id,
                    Address = m.Address,
                    CategoryType = ApiType.Promotion,
                    Title = m.Name
                }));
            }
            var count = allItems.Count;
            var start = Common.GetPaging(page, PageSize, count);
            return new
            {
                totalPage = ((count - 1) / PageSize) + 1,
                data = count > 0 ? allItems.OrderBy(m => m.Title).Skip(start).Take(PageSize).ToList() : new List<SearchAllModel>()
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}