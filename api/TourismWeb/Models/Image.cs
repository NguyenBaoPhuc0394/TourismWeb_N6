using System.ComponentModel.DataAnnotations;

namespace TourismWeb.Models
{
    public class Image
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [MaxLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Tour Id is required")]
        public string Tour_Id { get; set; }
        public Tour Tour { get; set; }
    }
}
