using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Image;
using TourismWeb.Services.Interfaces;
using NLog; // Thêm namespace cho NLog

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // Khởi tạo logger

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
            Logger.Debug("ImageController initialized.");
        }

        // GET: api/Image/all-images
        [HttpGet("all-images")]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetAllImages()
        {
            Logger.Info("Fetching all images.");
            try
            {
                var result = await _imageService.GetAllImages();
                Logger.Debug($"Retrieved {result.Count()} images.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching all images.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Image/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> GetImageById(string id)
        {
            Logger.Info($"Fetching image with ID: {id}");
            try
            {
                var image = await _imageService.GetImageById(id);
                Logger.Debug($"Image retrieved: {id}");
                return Ok(image);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Image with ID {id} not found: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching image with ID: {id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // POST: api/Image/create
        [HttpPost("create")]
        public async Task<ActionResult<ImageDTO>> CreateImage([FromForm] ImageUploadDTO data)
        {
            Logger.Info($"Image creation attempt for tour ID: {data.TourId}");
            try
            {
                string tourId = data.TourId;
                IFormFile file = data.File;
                if (file == null || file.Length == 0)
                {
                    Logger.Warn($"Image creation failed for tour ID: {tourId}. Invalid file.");
                    return BadRequest(new { status = 400, message = "File ảnh không hợp lệ!" });
                }

                Logger.Debug($"Uploading file: {file.FileName} for tour ID: {tourId}");
                var result = await _imageService.CreateImage(tourId, file);
                Logger.Info($"Image created successfully for tour ID: {tourId}, image ID: {result.Id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Image creation failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy tour!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error creating image for tour ID: {data.TourId}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // PUT: api/Image/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ImageDTO>> UpdateImageById(string id, ImageDTO updatedImage)
        {
            Logger.Info($"Updating image with ID: {id}");
            try
            {
                var result = await _imageService.UpdateImageById(id, updatedImage);
                Logger.Info($"Image updated successfully: {id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Image update failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating image with ID: {id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Image/get-tour
        [HttpGet("get-tour")]
        public async Task<ActionResult<IEnumerable<ImageTourDTO>>> GetImagesByTourId([FromQuery] string tourId)
        {
            Logger.Info($"Fetching images for tour ID: {tourId}");
            try
            {
                var images = await _imageService.GetImagesByTourId(tourId);
                Logger.Debug($"Retrieved {images.Count()} images for tour ID: {tourId}");
                return Ok(images);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Images not found for tour ID: {tourId}. {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching images for tour ID: {tourId}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Image/get-image-data
        [HttpGet("get-image-data")]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImagesDataByTourId([FromQuery] string tourId)
        {
            Logger.Info($"Fetching image data for tour ID: {tourId}");
            try
            {
                var images = await _imageService.GetImagesDataByTourId(tourId);
                Logger.Debug($"Retrieved {images.Count()} image data for tour ID: {tourId}");
                return Ok(images);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Image data not found for tour ID: {tourId}. {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching image data for tour ID: {tourId}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // DELETE: api/Image/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImageById(string id)
        {
            Logger.Info($"Deleting image with ID: {id}");
            try
            {
                var result = await _imageService.DeleteImageById(id);
                if (!result)
                {
                    Logger.Warn($"Image deletion failed: Image ID {id} not found or could not be deleted.");
                    return NotFound(new { message = "Không tìm thấy hoặc không thể xoá ảnh!" });
                }

                Logger.Info($"Image deleted successfully: {id}");
                return Ok(new { message = "Xoá ảnh thành công!" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting image with ID: {id}");
                return StatusCode(500, new { message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }
    }
}