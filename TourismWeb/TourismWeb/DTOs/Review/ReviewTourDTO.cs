namespace TourismWeb.DTOs.Review
{
    public class ReviewTourDTO
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreateAt { get; set; }
        public string CustomerName { get; set; }
    }
}
