using AutoMapper;
using TourismWeb.DTOs.Category;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> CreateCategory(CategoryCreateDTO createDTO)
        {
            var category = _mapper.Map<Category>(createDTO);
            var createdCategory = await _categoryRepository.CreateCategory(category);
            return _mapper.Map<CategoryDTO>(createdCategory);
        }

        public async Task DeleteCategoryById(string id)
        {
            await _categoryRepository.DeleteCategoryById(id);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var result = await _categoryRepository.GetAllCategories();
            return _mapper.Map<IEnumerable<CategoryDTO>>(result);
        }

        public async Task<CategoryDTO> GetCategoryById(string id)
        {
            var result = await _categoryRepository.GetCategoryById(id);
            return _mapper.Map<CategoryDTO>(result);
        }

        public async Task<CategoryDTO> UpdateCategoryById(string id, CategoryDTO category)
        {
            var result = await _categoryRepository.UpdateCategoryById(id, _mapper.Map<Category>(category));
            return _mapper.Map<CategoryDTO>(result);
        }
    }
}
