using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarMap.Models
{
    public class GeneralModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
        public string Name { get; set; }
        public string Address { get; set; }

        [Phone(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PhoneNumberError")]
        public string Mobile { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
        [RegularExpression(@"^(\-?\d+(\.\d+)?),\s*(\-?\d+(\.\d+)?)$", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LocationError")]
        public string Location { get; set; }
        public string ThumbImage { get; set; }
        public string DetailImage { get; set; }
        public string ThumbDescription { get; set; }
        public string DetailDescription { get; set; }
        //public Nullable<System.DateTime> PublicDate { get; set; }
        //public System.DateTime CreateDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
        public int CategoryId { get; set; }
        //public string CreatedBy { get; set; }
        //public string Lang { get; set; }
    }
}