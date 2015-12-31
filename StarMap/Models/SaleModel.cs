using System;

namespace StarMap.Models
{
    public class SaleModel : GeneralModel
    {
        public bool? IsHot {get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}