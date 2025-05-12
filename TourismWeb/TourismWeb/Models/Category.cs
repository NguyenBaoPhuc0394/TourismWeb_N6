using System.ComponentModel.DataAnnotations;

namespace TourismWeb.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category description is required")]
        public string Description { get; set; }
        public ICollection<Tour> Tours { get; set; }
    }
}
