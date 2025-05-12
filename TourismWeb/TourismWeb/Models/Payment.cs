using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace TourismWeb.Models
{
    public class Payment
    {
        [Key]
        public string Id { set; get; }

        [Required(ErrorMessage = "Booking Id is required!")]
        public string Booking_Id { set; get; }
        public Booking Booking { get; set; }

        [Required(ErrorMessage = "Amount is required!")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0.")]
        public decimal Amount { set; get; }
        /*

        [Required(ErrorMessage = "Method is required!")]
        public int Method {  set; get; }
        */
        [Required(ErrorMessage = "Status is required!")]
        public int Status { set; get; }
        public DateTime? Payment_date { set; get; }
    }
}
