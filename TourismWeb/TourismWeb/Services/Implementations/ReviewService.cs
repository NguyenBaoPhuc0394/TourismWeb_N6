using AutoMapper;
using TourismWeb.DTOs.Review;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class ReviewService: IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IBookingService _bookingService;

        public ReviewService(IReviewRepository paymentRepository, IMapper mapper, IBookingService bookingService)
        {
            _reviewRepository = paymentRepository;
            _mapper = mapper;
            _bookingService = bookingService;
        }

        public async Task<IEnumerable<ReviewTourDTO>> GetReviewTour(string tourID)
        {
            var reviews = await _reviewRepository.GetReviewsByTour(tourID);
            return reviews;
        }

        public async Task<ReviewDTO> CreateReview(ReviewCreateDTO data)
        {
            try
            {
                var customerId = await _bookingService.GetCusIdByBooking(data.BookId);
                var tourId = await _bookingService.GetTourIdByBooking(data.BookId);
                if (String.IsNullOrEmpty(customerId) || String.IsNullOrEmpty(tourId)) throw new KeyNotFoundException("Không thể tìm thấy customerId hoặc tourId tương ứng");
                var reviewData = new ReviewDTO
                {
                    Tour_Id = tourId,
                    Customer_Id = customerId,
                    Rating = data.Rating,
                    Comment = data.Comment,
                    Create_at = DateTime.Now
                };
                var result = await _reviewRepository.CreateReview(reviewData);
                var updateBooking = await _bookingService.UpadateBookingReviewed(data.BookId);
                if(!updateBooking) throw new Exception("Update trạng thái booking thất bại");
                return _mapper.Map<ReviewDTO>(result);
            } catch(Exception)
            {
                throw;
            }
           
        }
    }
}
