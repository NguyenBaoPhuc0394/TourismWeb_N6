namespace TourismWeb.DTOs.Customer
{
    public class CustomerDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
