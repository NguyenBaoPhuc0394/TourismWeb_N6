
namespace TourismWeb.DTOs.Review
{
    public class ReviewDTO
    {
        public string Tour_Id { set; get; }
        public string Customer_Id { set; get; }
        public int Rating { set; get; }
        public string Comment { set; get; }
        public DateTime Create_at { set; get; }
    }
}
