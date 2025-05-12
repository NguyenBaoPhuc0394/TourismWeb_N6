namespace TourismWeb.DTOs.Schedule
{
    public class ScheduleCreateDTO
    {
        public string Tour_Id { get; set; }
        public DateOnly Start_date { get; set; }
        public decimal Adult_price { get; set; }
        public decimal Children_price { get; set; }
        public double Discount {  get; set; }
    }
}
