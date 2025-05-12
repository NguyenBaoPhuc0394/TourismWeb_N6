namespace TourismWeb.DTOs.Review
{
    public class ReviewCreateDTO
    {
        public string BookId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
