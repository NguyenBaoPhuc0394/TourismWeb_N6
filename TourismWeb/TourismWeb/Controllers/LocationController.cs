using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Location;
using TourismWeb.Models;
using TourismWeb.Services.Implementations;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<LocationDTO>> CreateLocation(LocationCreateDTO createDTO)
        {
            try
            {
                var result = await _locationService.CreateLocation(createDTO);
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

        [HttpGet("AllLocation")]
        public async Task<ActionResult<IEnumerable<Location>>> GetALlLocation()
        {
            var result = await _locationService.GetAllLocation();
            if(result == null) return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!"});
            return Ok(result);
        }
    }
}
