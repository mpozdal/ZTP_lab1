namespace ProductApp.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.DTOs;
using ProductApp.Application.Interfaces;


[Controller]
[Route("api/v1/categories")]
public class CategoryController: ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var phrases =  await _categoryService.GetAllCategoriesAsync();
        return Ok(phrases);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var pharse = await _categoryService.GetCategoryByIdAsync(id);
        if (pharse == null)
            return NoContent();
        return Ok(pharse);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
    {
        if (categoryDto == null)
        return BadRequest("bad request");
        
        var createdProduct = await _categoryService.CreateCategoryAsync(categoryDto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _categoryService.DeleteCategoryAsync(id);
        if (!deleted)
        {
            return NotFound("not found");
        }

        return NoContent();
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDto categoryDto)
    {
        if (id != categoryDto.Id)
            return BadRequest("bad request");
        var updated = await _categoryService.UpdateCategoryAsync(categoryDto);
        if (!updated)
        {
            return NotFound("not found");
        }

        return NoContent();
    }
    
}