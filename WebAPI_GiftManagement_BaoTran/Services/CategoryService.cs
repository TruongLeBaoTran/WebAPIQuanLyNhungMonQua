using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategorys();
        Task<CategoryResponse> GetSingleCategory(int idCategory);
        Task<(bool Success, string ErrorMessage)> PostCategory(CategoryRequest categoryNew);
        Task<(bool Success, string ErrorMessage)> DeleteCategory(int id);
        Task<(bool Success, string ErrorMessage)> PutCategory(int id, CategoryRequest categoryUpdate);

    }

    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly CategoryValidator _validationCategory;
        private readonly IRepositoryWrapper _repository;

        public CategoryService(IMapper mapper, CategoryValidator validationRules, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _validationCategory = validationRules;
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategorys()
        {
            IEnumerable<Category> categorys = await _repository.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryResponse>>(categorys);
        }

        public async Task<CategoryResponse> GetSingleCategory(int idCategory)
        {
            Category category = await _repository.Categories.GetSingleAsync(g => g.IdCategory == idCategory);
            if (category == null) return null;

            return _mapper.Map<CategoryResponse>(category);
        }


        public async Task<(bool Success, string ErrorMessage)> PostCategory(CategoryRequest categoryNew)
        {

            if (await _repository.Categories.AnyAsync(g => g.Name == categoryNew.Name))
                return (false, "Name is already taken");

            FluentValidation.Results.ValidationResult validationResult = await _validationCategory.ValidateAsync(categoryNew);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            Category category = _mapper.Map<Category>(categoryNew);

            _repository.Categories.Create(category);
            await _repository.SaveChangeAsync();

            return (true, null);

        }

        public async Task<(bool Success, string ErrorMessage)> DeleteCategory(int id)
        {
            Category? category = await _repository.Categories.GetSingleAsync(u => u.IdCategory == id);
            if (category == null)
                return (false, "Category not found.");

            _repository.Categories.Delete(category);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> PutCategory(int id, CategoryRequest categoryUpdate)
        {
            Category existingCategory = await _repository.Categories.GetSingleAsync(g => g.IdCategory == id);
            if (existingCategory == null)
                return (false, "Category not found.");

            FluentValidation.Results.ValidationResult validationResult = await _validationCategory.ValidateAsync(categoryUpdate);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            if (await _repository.Categories.AnyAsync(g => g.Name == categoryUpdate.Name && g.IdCategory != id))
                return (false, "Name is already taken");

            _mapper.Map(categoryUpdate, existingCategory);

            _repository.Categories.Update(existingCategory);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

    }
}
