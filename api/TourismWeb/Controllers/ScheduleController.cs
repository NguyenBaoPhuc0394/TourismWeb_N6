using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Schedule;
using TourismWeb.Models;
using TourismWeb.Services.Implementations;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<ScheduleDTO>> CreateSchedule(ScheduleCreateDTO createDTO)
        {
            try
            {
                var result = await _scheduleService.CreateSchedule(createDTO);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        [HttpGet("by-tour")]
        public async Task<ActionResult<IEnumerable<ScheduleCalendarDTO>>> GetCalendarData([FromQuery] string tourID)
        {
            var result = await _scheduleService.GetCalendarData(tourID);

            if (result == null || !result.Any())
                return NotFound("Không tìm thấy lịch cho tour này.");

            return Ok(result);
        }

        [HttpGet("booking-data")]
        public async Task<ActionResult<ScheduleBookingDTO>> GetScheduleBookingData([FromQuery] string scheID)
        {
            var result = await _scheduleService.GetBookingData(scheID);

            if (result == null)
                return NotFound("Không tìm thấy lịch trình với ID tương ứng.");

            return Ok(result);
        }

        [HttpGet("table-data")]
        public async Task<ActionResult<IEnumerable<ScheduleDataTableDTO>>> GetDataForTable()
        {
            try
            {
                var data = await _scheduleService.GetScheduleWithTourNameAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    message = "Đã xảy ra lỗi khi lấy dữ liệu.",
                    detail = ex.Message
                });
            }
        }

        [HttpPut("{id}/discount")]
        public async Task<IActionResult> UpdateDiscount(string id, [FromQuery] float discount)
        {
            if (discount < 0 || discount >= 100)
            {
                return BadRequest(new { message = "Giảm giá không hợp lệ." });
            }

            try
            {
                await _scheduleService.UpdateScheduleDiscountAsync(id, discount);
                return Ok(new { message = "Cập nhật giảm giá thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật giảm giá.", error = ex.Message });
            }
        }


    }
}
