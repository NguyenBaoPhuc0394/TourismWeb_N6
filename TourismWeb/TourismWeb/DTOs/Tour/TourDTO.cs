namespace TourismWeb.DTOs.Tours
{
    public class TourDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Short_description { get; set; }
        public string Detail_description { get; set; }
        public string Schedule_description { get; set; }
        public string Category_Id { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
        public int Max_capacity { get; set; }
        public DateTime Create_at { get; set; }
        public DateTime Update_at { get; set; }
    }
}
