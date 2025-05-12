using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Image;
using TourismWeb.DTOs.Tours;
using TourismWeb.Services.Implementations;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // GET: api/Tour
        [HttpGet("all-images")]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetAllImages()
        {
            var result = await _imageService.GetAllImages();
            return Ok(result);
        }

        // GET: api/Tour/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> GetImageById(string id)
        {
            try
            {
                var image = await _imageService.GetImageById(id);
                return Ok(image);
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

        // POST: api/Tour
        [HttpPost("create")]
        public async Task<ActionResult<ImageDTO>> CreateImage(ImageDTO image)
        {
            try
            {
                var result = await _imageService.CreateImage(image);
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

        // PUT: api/Tour/id
        [HttpPut("{id}")]
        public async Task<ActionResult<ImageDTO>> UpdateImageById(string id, ImageDTO updatedImage)
        {
            try
            {
                var result = await _imageService.UpdateImageById(id, updatedImage);
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
        public async Task<ActionResult> DeleteImageById(string id)
        {
            try
            {
                await _imageService.DeleteImageById(id);
                return Ok(new { status = 200, message = "Xóa thành công!" });
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
        // GET: api/Tour/id
        [HttpGet("get-tour")]
        public async Task<ActionResult<IEnumerable<ImageTourDTO>>> GetImagesByTourId([FromQuery] string tourId)
        {
            try
            {
                var images = await _imageService.GetImagesByTourId(tourId);
                return Ok(images);
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

    }
}
