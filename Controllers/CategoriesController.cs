using Kt5RepositoryApi.Contracts;
using Kt5RepositoryApi.Models;
using Kt5RepositoryApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kt5RepositoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await unitOfWork.Categories.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id);
        return category is null ? NotFound(new { message = "Категория не найдена" }) : Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryRequest request)
    {
        var category = new Category { Name = request.Name.Trim() };
        await unitOfWork.Categories.AddAsync(category);
        await unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryRequest request)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound(new { message = "Категория не найдена" });
        }

        category.Name = request.Name.Trim();
        unitOfWork.Categories.Update(category);
        await unitOfWork.SaveChangesAsync();
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound(new { message = "Категория не найдена" });
        }

        if (await unitOfWork.Products.AnyAsync(product => product.CategoryId == id))
        {
            return Conflict(new { message = "Нельзя удалить категорию, пока в ней есть товары" });
        }

        unitOfWork.Categories.Delete(category);
        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
