using Kt5RepositoryApi.Contracts;
using Kt5RepositoryApi.Models;
using Kt5RepositoryApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kt5RepositoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await unitOfWork.Products.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id);
        return product is null ? NotFound(new { message = "Товар не найден" }) : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductRequest request)
    {
        if (await unitOfWork.Categories.GetByIdAsync(request.CategoryId) is null)
        {
            return BadRequest(new { message = "Указанная категория не существует" });
        }

        var product = new Product
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Price = request.Price,
            CategoryId = request.CategoryId
        };
        await unitOfWork.Products.AddAsync(product);
        await unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductRequest request)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound(new { message = "Товар не найден" });
        }

        if (await unitOfWork.Categories.GetByIdAsync(request.CategoryId) is null)
        {
            return BadRequest(new { message = "Указанная категория не существует" });
        }

        product.Name = request.Name.Trim();
        product.Description = request.Description?.Trim();
        product.Price = request.Price;
        product.CategoryId = request.CategoryId;
        unitOfWork.Products.Update(product);
        await unitOfWork.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound(new { message = "Товар не найден" });
        }

        unitOfWork.Products.Delete(product);
        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
