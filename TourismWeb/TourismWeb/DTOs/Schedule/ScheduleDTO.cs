namespace TourismWeb.DTOs.Schedule
{
    public class ScheduleDTO
    {
        public string Id { get; set; }
        public string Tour_Id { get; set; }
        public DateOnly Start_date { get; set; }
        public int Current_booked {  get; set; }
        public int Status { get; set; }
        public decimal Adult_price { get; set; }
        public decimal Children_price { get; set; }
        public double Discount { get; set; }
        public DateTime Create_at { get; set; }
    }
}
