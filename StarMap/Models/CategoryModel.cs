using System.ComponentModel.DataAnnotations;

namespace StarMap.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
        public string Name { get; set; }
        public string Image { get; set; }
    }
}