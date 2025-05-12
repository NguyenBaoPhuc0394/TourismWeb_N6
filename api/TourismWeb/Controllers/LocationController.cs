using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Location;
using TourismWeb.Models;
using TourismWeb.Services.Interfaces;
using NLog; 

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
            Logger.Debug("LocationController initialized.");
        }

        // POST: api/Location/create
        [HttpPost("create")]
        public async Task<ActionResult<LocationDTO>> CreateLocation(LocationCreateDTO createDTO)
        {
            Logger.Info("Location creation attempt.");
            try
            {
                var result = await _locationService.CreateLocation(createDTO);
                Logger.Info($"Location created successfully: {result.Id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Location creation failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error creating location.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Location/AllLocation
        [HttpGet("AllLocation")]
        public async Task<ActionResult<IEnumerable<Location>>> GetALlLocation()
        {
            Logger.Info("Fetching all locations.");
            try
            {
                var result = await _locationService.GetAllLocation();
                if (result == null)
                {
                    Logger.Warn("No locations found.");
                    return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!" });
                }

                Logger.Debug($"Retrieved {result.Count()} locations.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching all locations.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }
    }
}