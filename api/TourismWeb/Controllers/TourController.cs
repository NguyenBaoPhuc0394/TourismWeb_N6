using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Tour;
using TourismWeb.Services.Interfaces;
using NLog;
using TourismWeb.DTOs.Tours;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // Khởi tạo logger

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
            Logger.Debug("TourController initialized.");
        }

        // POST: api/Tour/create
        [HttpPost("create")]
        public async Task<ActionResult<TourDTO>> CreateTour(TourCreateDTO createDTO)
        {
            Logger.Info("Tour creation attempt.");
            try
            {
                var result = await _tourService.CreateTour(createDTO);
                Logger.Info($"Tour created successfully: {result.Id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Tour creation failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error creating tour.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Tour/all-tours
        [HttpGet("all-tours")]
        public async Task<ActionResult<IEnumerable<TourDTO>>> GetAllTours()
        {
            Logger.Info("Fetching all tours.");
            try
            {
                var result = await _tourService.GetAllTours();
                Logger.Debug($"Retrieved {result.Count()} tours.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching all tours.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Tour/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TourDTO>> GetTourById(string id)
        {
            Logger.Info($"Fetching tour with ID: {id}");
            try
            {
                var tour = await _tourService.GetTourById(id);
                Logger.Debug($"Tour retrieved: {id}");
                return Ok(tour);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Tour with ID {id} not found: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching tour with ID: {id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // PUT: api/Tour/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<TourCreateDTO>> UpdateTourById(string id, TourUpdateDTO updatedTour)
        {
            Logger.Info($"Updating tour with ID: {id}");
            try
            {
                var result = await _tourService.UpdateTourById(id, updatedTour);
                Logger.Info($"Tour updated successfully: {id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Tour update failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating tour with ID: {id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // DELETE: api/Tour/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTourById(string id)
        {
            Logger.Info($"Deleting tour with ID: {id}");
            try
            {
                await _tourService.DeleteTourById(id);
                Logger.Info($"Tour deleted successfully: {id}");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Tour deletion failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting tour with ID: {id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Tour/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TourCardDTO>>> SearchTours([FromQuery] TourSearchDTO searchData)
        {
            Logger.Info("Searching tours.");
            try
            {
                var tours = await _tourService.SearchTours(searchData);
                if (tours == null)
                {
                    Logger.Warn("No tours found matching search criteria.");
                    return NotFound(new { status = 404, message = "Không tìm tour khuyến mãi nào cả!" });
                }

                Logger.Debug($"Retrieved {tours.Count()} tours from search.");
                return Ok(tours);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error searching tours.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Tour/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TourDTO>>> GetToursByCategory(string categoryId)
        {
            Logger.Info($"Fetching tours for category ID: {categoryId}");
            try
            {
                var result = await _tourService.GetToursByCategory(categoryId);
                Logger.Debug($"Retrieved {result.Count()} tours for category ID: {categoryId}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"No tours found for category ID: {categoryId}. {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching tours for category ID: {categoryId}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Tour/promotions
        [HttpGet("promotions")]
        public async Task<ActionResult<IEnumerable<TourCardDTO>>> GetAllTourPromotion()
        {
            Logger.Info("Fetching all promotion tours.");
            try
            {
                var result = await _tourService.GetAllPromotionTours();
                if (result == null)
                {
                    Logger.Warn("No promotion tours found.");
                    return NotFound(new { status = 404, message = "Không tìm tour khuyến mãi nào cả!" });
                }

                Logger.Debug($"Retrieved {result.Count()} promotion tours.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching promotion tours.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Tour/tourTableData
        [HttpGet("tourTableData")]
        public async Task<ActionResult<IEnumerable<TourTableDTO>>> GetAllTourDataForTable()
        {
            Logger.Info("Fetching all tour data for table.");
            try
            {
                var result = await _tourService.GetAllTourTable();
                Logger.Debug($"Retrieved {result.Count()} tours for table.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching tour data for table.");
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }
    }
}