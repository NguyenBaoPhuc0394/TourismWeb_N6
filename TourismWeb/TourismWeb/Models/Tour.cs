using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace TourismWeb.Models
{
    public class Tour
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Tour name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Short description is required")]
        public string Short_description { get; set; }

        [Required(ErrorMessage = "Detail description is required")]
        public string Detail_description { get; set; }

        [Required(ErrorMessage = "Schedule is required")]
        public string Schedule_description { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [RegularExpression(@"^\d+N\d+D$", ErrorMessage = "Duration must be in the format like 'aNbD' which a, b is number")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Max capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Max capacity must be greater than 0")]
        public int Max_capacity { get; set; }
        public DateTime Create_at { get; set; }
        public DateTime? Update_at { get; set; }

        [Required(ErrorMessage ="Location is required")]
        public string Location_Id { get; set; }
        public Location Location { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category_Id { get; set; }
        public Category Category { get; set; }

        public ICollection<Image> Images { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
