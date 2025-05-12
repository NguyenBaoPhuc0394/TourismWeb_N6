namespace TourismWeb.DTOs.Schedule
{
    public class ScheduleCalendarDTO
    {
        public string Id { get; set; }
        public DateOnly Start_date { get; set; }
        public decimal Adult_price { get; set; }
        public int Available { set; get; }

    }
}
