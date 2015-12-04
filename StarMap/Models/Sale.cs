//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StarMap.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sale
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
        public Nullable<System.DateTime> PublicDate { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> Rate { get; set; }
        public int CategoryId { get; set; }
        public string CreatedBy { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
