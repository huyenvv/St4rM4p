using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarMap.Models
{
    public class DashboardModel
    {
        public int GPCount { get; set; }
        public int GPActiveCount { get; set; }
        public int GPInActiveCount { get; set; }
        public List<GeneralObject> GPRatingList { get; set; }
        public List<GeneralObject> GPCategoriesList { get; set; }

        public int EventCount { get; set; }
        public int EventActiveCount { get; set; }
        public int EventInActiveCount { get; set; }
        public int EventHotCount { get; set; }
        public int EventNewCount { get; set; }
        public List<GeneralObject> EventCategoriesList { get; set; }

        public int SaleCount { get; set; }
        public int SaleActiveCount { get; set; }
        public int SaleInActiveCount { get; set; }
        public int SaleHotCount { get; set; }
        public int SaleNewCount { get; set; }
        public List<GeneralObject> SaleCategoriesList { get; set; }
    }
}