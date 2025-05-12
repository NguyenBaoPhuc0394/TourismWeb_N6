namespace TourismWeb.DTOs.Payment
{
    public class PaymentDTO
    {
        public string Id { get; set; }
        public string Booking_Id { get; set; }
        public decimal Amount { get; set; }
        public int Method { get; set; }
        public int Status { get; set; }
        public DateTime Payment_date { get; set; }
    }
}
