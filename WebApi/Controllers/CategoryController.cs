using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Domain.Models;
namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private CategoryService _categoryService;
    public CategoryController()
    {
        _categoryService = new CategoryService();
    }

    [HttpGet("GetCategories")]
    public List<Category> GetCategories()
    {
        return _categoryService.GetCategories();
    }

    [HttpPost("AddCategory")]
    public int AddCategory(Category category)
    {
        return _categoryService.AddCategory(category);
    }

    [HttpPut("UpdateCategory")]
    public int UpdateCategory(Category category)
    {
        return _categoryService.UpdateCategory(category);
    }

    [HttpDelete("DeleteCategory")]
    public int DeleteCategory(int id)
    {
        return _categoryService.DeleteCategory(id);
    }
}
