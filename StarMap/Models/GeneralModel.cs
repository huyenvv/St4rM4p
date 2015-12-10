using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarMap.Models
{
    public class GeneralModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Location { get; set; }
        public string ThumbImage { get; set; }
        public string DetailImage { get; set; }
        public string ThumbDescription { get; set; }
        public string DetailDescription { get; set; }
        //public Nullable<System.DateTime> PublicDate { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public int CategoryId { get; set; }
        //public string CreatedBy { get; set; }
        //public string Lang { get; set; }
    }
}