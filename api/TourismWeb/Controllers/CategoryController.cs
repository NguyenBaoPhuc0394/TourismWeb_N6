using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Category;
using TourismWeb.Services.Interfaces;
using NLog; 

namespace TourismWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            Logger.Debug("CategoryController initialized.");
        }

        // POST: api/Category/create
        [HttpPost("create")]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CategoryCreateDTO createDTO)
        {
            Logger.Info("Category creation attempt.");
            try
            {
                var result = await _categoryService.CreateCategory(createDTO);
                Logger.Info($"Category created successfully: {result.Id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Category creation failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error creating category.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Category/all-categories
        [HttpGet("all-categories")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            Logger.Info("Fetching all categories.");
            try
            {
                var result = await _categoryService.GetAllCategories();
                Logger.Debug($"Retrieved {result.Count()} categories.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching all categories.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Category/id
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(string id)
        {
            Logger.Info($"Fetching category with ID: {id}");
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                Logger.Debug($"Category retrieved: {id}");
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Category with ID {id} not found: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching category with ID: {id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // PUT: api/Category/id
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategoryById(string id, CategoryDTO category)
        {
            Logger.Info($"Updating category with ID: {id}");
            try
            {
                var result = await _categoryService.UpdateCategoryById(id, category);
                Logger.Info($"Category updated successfully: {id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Category update failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating category with ID: {id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // DELETE: api/Category/id
        [HttpDelete("{id}")]
        public ActionResult DeleteCategoryById(string id)
        {
            Logger.Info($"Deleting category with ID: {id}");
            try
            {
                _categoryService.DeleteCategoryById(id);
                Logger.Info($"Category deleted successfully: {id}");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Category deletion failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting category with ID: {id}");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }
    }
}