using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace TourismWeb.Models
{
    public class Customer
    {
        [Key]
        public string Id { set; get; }

        [Required(ErrorMessage = "Customer name is required!")]
        [StringLength(50, ErrorMessage = "Customer name cannot exceed 50 characters!")]
        public string? Name { set; get; }

        [Required(ErrorMessage = "Customer DateOfBirth is required!")]
        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth{ set; get; }

        [Required(ErrorMessage = "Customer gender is required!")]
        public int? Gender { set; get; }

        [Required(ErrorMessage = "Customer email is required!")]
        [StringLength(100, MinimumLength = 16, ErrorMessage = "Customer email must be between 16 and 100 characters!")]
        [EmailAddress]
        [RegularExpression(@".+@gmail.com$", ErrorMessage = "Email must end with @gmail.com")]
        public string Email {  set; get; }

        [Required(ErrorMessage = "Customer phone is required!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Customer phone must be exactly 10 digits")]
        public string? Phone { set; get; }
        public Account? Account { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
