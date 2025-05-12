using TourismWeb.DTOs.Review;
using TourismWeb.Models;

namespace TourismWeb.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewTourDTO>> GetReviewTour(string tourID);
        Task<ReviewDTO> CreateReview(ReviewCreateDTO data); 
    }
}
