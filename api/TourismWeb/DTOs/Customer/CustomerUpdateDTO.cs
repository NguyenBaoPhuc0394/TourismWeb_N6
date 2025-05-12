namespace TourismWeb.DTOs.Customer
{
    public class CustomerUpdateDTO
    {
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
