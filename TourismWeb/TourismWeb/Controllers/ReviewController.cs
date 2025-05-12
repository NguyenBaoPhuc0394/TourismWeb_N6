using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Review;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {

        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("get-tour")]
        public async Task<IActionResult> GetTourReviews([FromQuery] string tourID)
        {
            var reviews = await _reviewService.GetReviewTour(tourID);

            if (reviews == null || !reviews.Any())
            {
                return NotFound("Không tìm thấy đánh giá nào cho tour này.");
            }

            return Ok(reviews);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReviewDTO>> AddReview([FromBody] ReviewCreateDTO data)
        {
            try
            {
                var result = await _reviewService.CreateReview(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }
    }
}
