using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Schedule;
using TourismWeb.Models;
using TourismWeb.Services.Interfaces;
using NLog; // Thêm namespace cho NLog

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // Khởi tạo logger

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
            Logger.Debug("ScheduleController initialized.");
        }

        // POST: api/Schedule/create
        [HttpPost("create")]
        public async Task<ActionResult<ScheduleDTO>> CreateSchedule(ScheduleCreateDTO createDTO)
        {
            Logger.Info($"Schedule creation attempt for tour ID: {createDTO.Tour_Id}");
            try
            {
                var result = await _scheduleService.CreateSchedule(createDTO);
                Logger.Info($"Schedule created successfully: {result.Id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Schedule creation failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error creating schedule for tour ID: {createDTO.Tour_Id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Schedule/by-tour
        [HttpGet("by-tour")]
        public async Task<ActionResult<IEnumerable<ScheduleCalendarDTO>>> GetCalendarData([FromQuery] string tourID)
        {
            Logger.Info($"Fetching calendar data for tour ID: {tourID}");
            try
            {
                var result = await _scheduleService.GetCalendarData(tourID);
                if (result == null || !result.Any())
                {
                    Logger.Warn($"No schedules found for tour ID: {tourID}");
                    return NotFound("Không tìm thấy lịch cho tour này.");
                }

                Logger.Debug($"Retrieved {result.Count()} schedules for tour ID: {tourID}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching calendar data for tour ID: {tourID}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Schedule/booking-data
        [HttpGet("booking-data")]
        public async Task<ActionResult<ScheduleBookingDTO>> GetScheduleBookingData([FromQuery] string scheID)
        {
            Logger.Info($"Fetching booking data for schedule ID: {scheID}");
            try
            {
                var result = await _scheduleService.GetBookingData(scheID);
                if (result == null)
                {
                    Logger.Warn($"No schedule found for schedule ID: {scheID}");
                    return NotFound("Không tìm thấy lịch trình với ID tương ứng.");
                }

                Logger.Debug($"Booking data retrieved for schedule ID: {scheID}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching booking data for schedule ID: {scheID}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Schedule/table-data
        [HttpGet("table-data")]
        public async Task<ActionResult<IEnumerable<ScheduleDataTableDTO>>> GetDataForTable()
        {
            Logger.Info("Fetching schedule data for table.");
            try
            {
                var data = await _scheduleService.GetScheduleWithTourNameAsync();
                Logger.Debug($"Retrieved {data.Count()} schedules for table.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching schedule data for table.");
                return StatusCode(500, new
                {
                    status = 500,
                    message = "Đã xảy ra lỗi khi lấy dữ liệu.",
                    detail = ex.Message
                });
            }
        }

        // PUT: api/Schedule/{id}/discount
        [HttpPut("{id}/discount")]
        public async Task<IActionResult> UpdateDiscount(string id, [FromQuery] float discount)
        {
            Logger.Info($"Updating discount for schedule ID: {id}, discount: {discount}%");
            try
            {
                if (discount < 0 || discount >= 100)
                {
                    Logger.Warn($"Invalid discount value: {discount}% for schedule ID: {id}");
                    return BadRequest(new { message = "Giảm giá không hợp lệ." });
                }

                await _scheduleService.UpdateScheduleDiscountAsync(id, discount);
                Logger.Info($"Discount updated successfully for schedule ID: {id}");
                return Ok(new { message = "Cập nhật giảm giá thành công." });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating discount for schedule ID: {id}");
                return StatusCode(500, new { message = "Lỗi khi cập nhật giảm giá.", error = ex.Message });
            }
        }
    }
}