namespace TourismWeb.DTOs.Tour
{
    public class TourSearchDTO
    {
        public string? category { get; set; }
        public string? departureLocation { get; set; }
        public int? minPrice { get; set; }
        public int? maxPrice { get; set; }
        public DateTime? departureTime { get; set; }
    }
}
