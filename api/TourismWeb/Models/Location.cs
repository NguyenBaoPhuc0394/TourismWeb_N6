using System.ComponentModel.DataAnnotations;

namespace TourismWeb.Models
{
    public class Location
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Tour name is required")]
        [MaxLength(500, ErrorMessage = "Tour name cannot exceed 500 characters")]
        public string Name { get; set; }
        public ICollection<Tour> Tours { get; set; }

    }
}
