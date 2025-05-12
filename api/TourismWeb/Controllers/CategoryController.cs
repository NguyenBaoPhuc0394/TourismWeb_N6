using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Category;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // POST: api/Category
        [HttpPost("create")]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CategoryCreateDTO createDTO)
        {
            try
            {
                var result = await _categoryService.CreateCategory(createDTO);
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

        // GET: api/Category
        [HttpGet("all-categories")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategories();
            return Ok(result);
        }

        // GET: api/Category/id
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(string id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                return Ok(category);
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

        // PUT : api/Category/id
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategoryById(string id, CategoryDTO category)
        {
            try
            {
                var result = await _categoryService.UpdateCategoryById(id, category);
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

        // DELETE: api/Category/id
        [HttpDelete("{id}")]
        public ActionResult DeleteCategoryById(string id)
        {
            try
            {
                _categoryService.DeleteCategoryById(id);
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
    }
}
