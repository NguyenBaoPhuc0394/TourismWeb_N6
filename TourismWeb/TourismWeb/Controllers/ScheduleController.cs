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
    }
}
