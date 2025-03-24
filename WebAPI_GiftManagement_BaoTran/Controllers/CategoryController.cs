using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        [CustomAuthorize("ViewCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            return Ok(await _categoryService.GetAllCategorys());
        }


        [HttpGet("{idCategory}")]
        [CustomAuthorize("ViewOneCategory")]
        public async Task<IActionResult> GetOneCategory(int idCategory)
        {
            CategoryResponse category = await _categoryService.GetSingleCategory(idCategory);
            if (category == null)
            {
                return BadRequest("Not found");
            }
            return Ok(category);
        }


        [HttpPost]
        [CustomAuthorize("CreateCategory")]

        public async Task<IActionResult> PostCategory([FromForm] CategoryRequest category)
        {
            (bool Success, string ErrorMessage) result = await _categoryService.PostCategory(category);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data added successfully");
        }


        [HttpDelete("{id}")]
        [CustomAuthorize("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            (bool Success, string ErrorMessage) result = await _categoryService.DeleteCategory(id);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data deleted successfully");
        }

        [HttpPut("{id}")]
        [CustomAuthorize("UpdateCategory")]
        public async Task<IActionResult> PutCategory(int id, [FromForm] CategoryRequest userUpdate)
        {
            (bool Success, string ErrorMessage) result = await _categoryService.PutCategory(id, userUpdate);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data updated successfully");
        }

    }
}
