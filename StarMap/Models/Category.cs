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
    
    public partial class Category
    {
        public Category()
        {
            this.Event = new HashSet<Event>();
            this.Sale = new HashSet<Sale>();
            this.GoldPoint = new HashSet<GoldPoint>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Lang { get; set; }
    
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual ICollection<GoldPoint> GoldPoint { get; set; }
    }
}
