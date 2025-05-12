using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TourismWeb.Models
{
    public class Account
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage ="Username must be between 6 and 100 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, MinimumLength = 16, ErrorMessage = "Email must be between 16 and 100 characters")]
        [EmailAddress]
        [RegularExpression(@".+@gmail.com$", ErrorMessage = "Email must end with @gmail.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int Role { get; set; }
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be exactly 6 digits")]

        public string? OTP { get; set; }

        [Range(0, 1, ErrorMessage = "isConfirmed must be 0 (false) or 1 (true)")]
        public int isConfirmed {  get; set; }
        public Customer? Customer { get; set; }
    }
}
