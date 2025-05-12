namespace TourismWeb.DTOs.Tour
{
    public class TourCardDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public decimal TourPrice { get; set; }
        public decimal? LowestAdultPrice { get; set; }
        public string? ImageUrl { get; set; }
        public float? AverageRating {  get; set; }
    }
}
