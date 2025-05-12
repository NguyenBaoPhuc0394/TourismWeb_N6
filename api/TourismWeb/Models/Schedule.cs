using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace TourismWeb.Models
{
    public class Schedule
    {
        [Key]
        public string Id { set; get; }

        [Required(ErrorMessage = "Start date is required!")]
        public DateOnly Start_date { set; get; }

        [Required(ErrorMessage = "Available is required!")]
        public int Available {  set; get; }

        [Required(ErrorMessage = "Status is required!")]
        public int Status { set; get; }

        [Required(ErrorMessage = "Adult price is required!")]
        [Range(0, double.MaxValue, ErrorMessage = "Adult price must be greater than or equal to 0")]
        public decimal Adult_price { set; get; }

        [Required(ErrorMessage = "Children price is required!")]
        [Range(0, double.MaxValue, ErrorMessage = "Children price must be greater than or equal to 0")]
        public decimal Children_price { set; get; }

        [Required(ErrorMessage = "Discount is required!")]
        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0% and 100%!")]
        public double Discount {  set; get; }
        public DateTime Create_at { set; get; }

        [Required(ErrorMessage = "Tour Id is required!")]
        public string Tour_Id { set; get; }
        public Tour Tour { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
