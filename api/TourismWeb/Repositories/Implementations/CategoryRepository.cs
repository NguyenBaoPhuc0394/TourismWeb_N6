using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;

namespace TourismWeb.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TourismDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(TourismDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            var result = _context.Categories.FromSqlRaw("Exec CreateCategory @Name = {0}, @Description = {1}", category.Name, category.Description).AsEnumerable().FirstOrDefault();
            if (result == null)
            {
                throw new Exception("Category not found!");
            }
            return result;
        }

        public async Task DeleteCategoryById(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found!");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found!");
            }
            return category;
        }

        public async Task<Category> UpdateCategoryById(string id, Category updatedCategory)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Category not found!");
            }

            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;

            await _context.SaveChangesAsync();

            return existingCategory;
        }
    }
}
