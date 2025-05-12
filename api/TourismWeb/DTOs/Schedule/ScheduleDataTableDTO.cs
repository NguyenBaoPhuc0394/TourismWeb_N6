namespace TourismWeb.DTOs.Schedule
{
    public class ScheduleDataTableDTO
    {
        public string Id { get; set; }
        public string TourName { get; set; }
        public DateOnly StartDate { get; set; }
        public int Available { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildrenPrice { get; set; }
        public double Discount { get; set; }
    }
}
