using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace TourismWeb.Models
{
    public class Booking
    {
        [Key]
        public string Id { set; get; }

        [Required(ErrorMessage = "Customer Id is required!")]
        public string Customer_Id { set; get; }

        [Required(ErrorMessage = "Schedule Id is required!")]
        public string Schedule_Id { set; get; }

        [Required(ErrorMessage = "Booking date is required!")]
        public DateTime Booking_date { set; get; }

        [Required(ErrorMessage = "Number of adult bookings is required!")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of adult bookings must be a non-negative number.")]
        public int Number_of_adult_bookings { set; get; }

        [Required(ErrorMessage = "Number of children bookings is required!")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of children bookings must be a non-negative number.")]
        public int Number_of_children_bookings { set; get; }

        [Required(ErrorMessage = "Total price is required!")]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be greater than or equal to 0.")]
        public decimal Total_price { set; get; }

        [Required(ErrorMessage = "Status is required!")]
        public int Status { set; get; }
        public Customer Customer { get; set; }
        public Schedule Schedule { get; set; }
        public Payment Payment { get; set; }

    }
}
