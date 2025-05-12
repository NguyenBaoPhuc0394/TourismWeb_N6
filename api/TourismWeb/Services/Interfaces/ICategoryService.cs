using TourismWeb.DTOs.Category;
using TourismWeb.Models;

namespace TourismWeb.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategory(CategoryCreateDTO createDTO);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> GetCategoryById(string id);
        Task<CategoryDTO> UpdateCategoryById(string id, CategoryDTO category);
        Task DeleteCategoryById(string id);
    }
}
