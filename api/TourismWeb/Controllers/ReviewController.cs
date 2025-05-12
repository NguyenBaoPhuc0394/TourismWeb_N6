using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Review;
using TourismWeb.Services.Interfaces;
using NLog; 

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); 

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
            Logger.Debug("ReviewController initialized.");
        }

        // GET: api/Review/get-tour
        [HttpGet("get-tour")]
        public async Task<IActionResult> GetTourReviews([FromQuery] string tourID)
        {
            Logger.Info($"Fetching reviews for tour ID: {tourID}");
            try
            {
                var reviews = await _reviewService.GetReviewTour(tourID);
                if (reviews == null || !reviews.Any())
                {
                    Logger.Warn($"No reviews found for tour ID: {tourID}");
                    return NotFound("Không tìm thấy đánh giá nào cho tour này.");
                }

                Logger.Debug($"Retrieved {reviews.Count()} reviews for tour ID: {tourID}");
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching reviews for tour ID: {tourID}");
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }

        // POST: api/Review
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReviewDTO>> AddReview([FromBody] ReviewCreateDTO data)
        {
            Logger.Info($"Review creation attempt for book ID: {data.BookId}");
            try
            {
                var result = await _reviewService.CreateReview(data);
                Logger.Info($"Review created successfully for book ID: {data.BookId}, review tour ID: {result.Tour_Id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error creating review for book ID: {data.BookId}");
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }
    }
}