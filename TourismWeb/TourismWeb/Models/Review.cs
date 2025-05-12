using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace TourismWeb.Models
{
    public class Review
    {
        [Key]
        public string Id { set; get; }

        [Required(ErrorMessage = "Tour Id is required!")]
        public string Tour_Id { set; get; }

        [Required(ErrorMessage = "Customer Id is required!")]
        public string Customer_Id { set; get; }

        [Required(ErrorMessage = "Rating is required!")]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5 points!")]
        public int Rating { set; get; }

        [Required(ErrorMessage = "Comment is required!")]
        [StringLength(5000, ErrorMessage = "Comment cannot exceed 5000 characters!")]
        public string Comment { set; get; }
        public DateTime Create_at { set; get; }
        public Tour Tour { get; set; }
        public Customer Customer { get; set; }

    }
}
