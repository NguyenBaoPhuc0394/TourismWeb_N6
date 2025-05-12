namespace TourismWeb.DTOs.Tour
{
    public class TourUpdateDTO
    {
        public string Name { get; set; }
        public string Short_description { get; set; }
        public string Detail_description { get; set; }
        public string Schedule_description { get; set; }
        public string Duration { get; set; }
        public int Max_capacity { get; set; }
    }
}
