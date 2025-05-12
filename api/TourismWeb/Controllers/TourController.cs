using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TourismWeb.DTOs.Tour;
using TourismWeb.DTOs.Tours;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;
        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        // POST: api/Tour
        [HttpPost("create")]
        public async Task<ActionResult<TourDTO>> CreateTour(TourCreateDTO createDTO)
        {
            try
            {
                var result = await _tourService.CreateTour(createDTO);
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

        // GET: api/Tour
        [HttpGet("all-tours")]
        public async Task<ActionResult<IEnumerable<TourDTO>>> GetAllTours()
        {
            var result = await _tourService.GetAllTours();
            return Ok(result);
        }

        // GET: api/Tour/id
        [HttpGet("{id}")]
        public async Task<ActionResult<TourDTO>> GetTourById(string id)
        {
            try
            {
                var tour = await _tourService.GetTourById(id);
                return Ok(tour);
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

        // PUT : api/Tour/id
        [HttpPut("{id}")]
        public async Task<ActionResult<TourCreateDTO>> UpdateTourById(string id, TourUpdateDTO updatedTour)
        {
            try
            {
                var result = await _tourService.UpdateTourById(id, updatedTour);
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

        // DELETE: api/Tour/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTourById(string id)
        {
            try
            {
                await _tourService.DeleteTourById(id);
                return NoContent();
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

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TourCardDTO>>> SearchTours([FromQuery] TourSearchDTO searchData)
         {
            var tours = await _tourService.SearchTours(searchData);
            if(tours == null) 
                return NotFound(new { status = 404, message = "Không tìm tour khuyến mãi nào cả!" });
            return Ok(tours);
        }

        // GET: api/Tour/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TourDTO>>> GetToursByCategory(string categoryId)
        {
            try
            {
                var result = await _tourService.GetToursByCategory(categoryId);
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

        [HttpGet("promotions")]
        public async Task<ActionResult<IEnumerable<TourCardDTO>>> GetAllTourPromotion()
        {
            var result = await _tourService.GetAllPromotionTours();
            if(result == null) 
                return NotFound(new { status = 404, message = "Không tìm tour khuyến mãi nào cả!"});
            return Ok(result);
        }

        [HttpGet("tourTableData")]
        public async Task<ActionResult<IEnumerable<TourTableDTO>>> GetAllTourDataForTable()
        {
            try
            {
                var result = await _tourService.GetAllTourTable();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra", detail = ex.Message });
            }
        }
    }
}
