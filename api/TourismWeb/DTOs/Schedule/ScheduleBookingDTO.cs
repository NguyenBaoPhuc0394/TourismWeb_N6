namespace TourismWeb.DTOs.Schedule
{
    public class ScheduleBookingDTO
    {
        public string Id { get; set; }
        public string TourName { get; set; }
        public string Duration { get; set; }
        public int Available { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
    }
}
