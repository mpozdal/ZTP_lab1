using Microsoft.EntityFrameworkCore;
using ProductApp.Application.DTOs;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Interfaces;
using ProductApp.Application.Interfaces;

namespace ProductApp.Application.Services;

public class CategoryService: ICategoryService
{

    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllAsync();
       
    }
    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var product = await _categoryRepository.GetByIdAsync(id);
        if (product == null) return null;

        return new CategoryDto()
        {
            Id = product.Id,
            Name = product.Name,
            MinPrice = product.MinPrice,
            MaxPrice = product.MaxPrice
            
        };
    }
    
    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
     
        var category = new Category()
        {
            Name = categoryDto.Name,
            MinPrice = categoryDto.MinPrice,
            MaxPrice = categoryDto.MaxPrice,
        };
        await _categoryRepository.AddAsync(category);

        categoryDto.Id = category.Id;

        return categoryDto;
    }

    public async Task<bool> UpdateCategoryAsync(CategoryDto categoryDto)
    {
        var existingProduct = await _categoryRepository.GetByIdAsync(categoryDto.Id);
        if (existingProduct == null) return false;

        existingProduct.Name = categoryDto.Name;
        existingProduct.MinPrice = categoryDto.MinPrice;
        existingProduct.MaxPrice = categoryDto.MaxPrice;
        existingProduct.UpdatedAt = DateTime.Now;

        await _categoryRepository.UpdateAsync(existingProduct);
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var exisitingPharse = await _categoryRepository.GetByIdAsync(id);
        if (exisitingPharse == null)
        {
            return false;
        }
        await _categoryRepository.DeleteAsync(id);
        return true;
    }
    
}