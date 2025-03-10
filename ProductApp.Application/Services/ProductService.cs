using System.Diagnostics;
using System.Globalization;
using ProductApp.Application.DTOs;
using ProductApp.Application.Policies;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Policies;
using ProductApp.Application.Interfaces;
using ProductApp.Domain.Excetpions;
using ProductApp.Domain.Validation;

namespace ProductApp.Application.Services;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IForbiddenPhraseService _forbiddenPhraseService;
    private readonly IProductChangeHistoryService _productChangeHistoryService;
    private readonly List<IValidationPolicy> _policies;
    
    public ProductService(IProductRepository productRepository, IForbiddenPhraseService forbiddenPhraseService, IProductChangeHistoryService productChangeHistoryService, IEnumerable<IValidationPolicy> policies)
    {
        _productRepository = productRepository;
        _forbiddenPhraseService = forbiddenPhraseService;
        _productChangeHistoryService = productChangeHistoryService;
        _policies = policies.ToList();

    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products;
    }
    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            Price = product.Price,
            Stock = product.Stock
        };
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
    {
        var validationResult = await ValidateProductAsync(productDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            CategoryId = productDto.CategoryId,
            Price = productDto.Price,
            Stock = productDto.Stock
        };
        await _productRepository.AddAsync(product);

        productDto.Id = product.Id;

        return productDto;
        
    }
    
    public async Task<bool> UpdateProductAsync(ProductDto productDto)
{
    var validationResult = await ValidateProductAsync(productDto);
    if (!validationResult.IsValid)
    {
        throw new ValidationException(validationResult.Errors);
    }
    
    var existingProduct = await _productRepository.GetByIdAsync(productDto.Id);
    if (existingProduct == null) return false;
    
    if (existingProduct.Name != productDto.Name)
    {
        await LogProductChangeAsync(existingProduct, "Name", existingProduct.Name, productDto.Name);
        existingProduct.Name = productDto.Name;
    }

    if (existingProduct.Description != productDto.Description)
    {
        await LogProductChangeAsync(existingProduct, "Desc", existingProduct.Description, productDto.Description);
        existingProduct.Description = productDto.Description;
    }

    if (existingProduct.Price != productDto.Price)
    {
        await LogProductChangeAsync(existingProduct, "Price", existingProduct.Price.ToString(), productDto.Price.ToString());
        existingProduct.Price = productDto.Price;
    }

    if (existingProduct.Stock != productDto.Stock)
    {
        await LogProductChangeAsync(existingProduct, "Stock", existingProduct.Stock.ToString(), productDto.Stock.ToString());
        existingProduct.Stock = productDto.Stock;
    }

    if (existingProduct.CategoryId != productDto.CategoryId)
    {
        await LogProductChangeAsync(existingProduct, "Category", existingProduct.CategoryId.ToString(), productDto.CategoryId.ToString());
        existingProduct.CategoryId = productDto.CategoryId;
    }
    
    existingProduct.UpdatedAt = DateTime.Now;
    
    await _productRepository.UpdateAsync(existingProduct);
    return true;
}

private async Task LogProductChangeAsync(Product existingProduct, string changeType, string oldValue, string newValue)
{
    var changeHistory = new ProductChangeHistory
    {
        Id = Guid.NewGuid(),
        ProductId = existingProduct.Id,
        ChangeDate = DateTime.UtcNow,
        ChangeType = changeType,
        OldValue = oldValue,
        NewValue = newValue
    };

    await _productChangeHistoryService.CreateProductHistoryAsync(changeHistory);
    
}


    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) return false;

        await _productRepository.DeleteAsync(id);
        return true;
    }
    
    private async Task<ValidationResult> ValidateProductAsync(ProductDto productDto)
    {
        var validationResult = new ValidationResult();
        
        foreach (var policy in _policies)
        {
            ValidationResult result;
            if (policy is NonNegativeStockPolicy)
            {
                result = await policy.Validate(productDto.Stock.ToString(), Guid.Empty);
                validationResult.Errors.AddRange(result.Errors);
                continue;
            }

            if (policy is UniqueNamePolicy)
            {
                result = await policy.Validate(productDto.Name, productDto.Id);
                validationResult.Errors.AddRange(result.Errors);
                continue;
            }

            if (policy is PriceRangePolicy)
            {
                result = await policy.Validate(productDto.Price.ToString(), productDto.CategoryId);
                validationResult.Errors.AddRange(result.Errors);
                continue;
            }
            result = await policy.Validate(productDto.Name, Guid.Empty);
            validationResult.Errors.AddRange(result.Errors);
        }

        return validationResult;

    }


    
}