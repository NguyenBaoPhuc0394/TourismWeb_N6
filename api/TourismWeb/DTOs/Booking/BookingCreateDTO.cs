namespace TourismWeb.DTOs.Booking
{
    public class BookingCreateDTO
    {
        public string Customer_Id { get; set; }
        public string Schedule_Id { get; set; }
        public int Number_of_adult_bookings { get; set; }
        public int Number_of_children_bookings { get; set; }

        public decimal Total_price {  get; set; }
    }
}
