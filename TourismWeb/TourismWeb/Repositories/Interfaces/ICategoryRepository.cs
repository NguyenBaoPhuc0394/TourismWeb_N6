using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategory(Category category);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(string id);
        Task<Category> UpdateCategoryById(string id, Category category);
        Task DeleteCategoryById(string id);
    }
}
