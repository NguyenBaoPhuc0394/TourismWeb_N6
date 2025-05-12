using TourismWeb.DTOs.Review;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<ReviewTourDTO>> GetReviewsByTour(string tourId);

        Task<Review> CreateReview(ReviewDTO data);
    }
}
