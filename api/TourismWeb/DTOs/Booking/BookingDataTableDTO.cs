namespace TourismWeb.DTOs.Booking
{
    public class BookingDataTableDTO
    {
        public string Id { get; set; }
        public string Customer_Id { get; set; }
        public DateTime Booking_date { get; set; }
        public int Number_of_adult_bookings { get; set; }
        public int Number_of_children_bookings { get; set; }
        public decimal Total_price { get; set; }
        public int Status { get; set; }
    }
}
