using TourismWeb.Data;
using TourismWeb.DTOs.Review;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TourismWeb.Repositories.Implementations
{
    public class ReviewRepository: IReviewRepository
    {
        private readonly TourismDbContext _context;

        public ReviewRepository(TourismDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewTourDTO>> GetReviewsByTour(string tourId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.Tour_Id == tourId)
                .Select(r => new ReviewTourDTO
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    CreateAt = r.Create_at,
                    CustomerName = r.Customer.Name
                })
                .ToListAsync();

            return reviews;
        }

        public async Task<Review> CreateReview(ReviewDTO data)
        {
            var result = _context.Reviews
                .FromSqlRaw("EXEC CreateReview @TourId = {0}, @CustomerId = {1}, @Rating = {2}, @Comment = {3}",
                    data.Tour_Id,
                    data.Customer_Id,
                    data.Rating,
                    data.Comment)
                .AsEnumerable()
                .FirstOrDefault();

            if (result == null)
            {
                throw new Exception("Không thể tạo đánh giá.");
            }

            return result;
        }

    }
}
