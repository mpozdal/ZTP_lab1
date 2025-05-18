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
using ProductApp.Infrastructure.Repositories;

namespace ProductApp.Application.Services;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductReservationRepository _productReservation;
    private readonly IProductChangeHistoryService _productChangeHistoryService;
    private readonly List<IValidationPolicy> _policies;
    
    public ProductService(IProductRepository productRepository, ProductReservationRepository productReservation, IProductChangeHistoryService productChangeHistoryService, IEnumerable<IValidationPolicy> policies)
    {
        _productRepository = productRepository;
        _productReservation = productReservation;
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
            Stock = productDto.Stock,
            Id = new Guid()
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
    var properties = typeof(ProductDto).GetProperties();

    foreach (var prop in properties)
    {
        var oldValue = prop.GetValue(existingProduct);
        var newValue = prop.GetValue(productDto);

        if (!object.Equals(oldValue, newValue))
        {
            await LogProductChangeAsync(existingProduct, prop.Name, oldValue?.ToString(), newValue?.ToString());
            prop.SetValue(existingProduct, newValue);
        }
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
            switch (policy)
            {
                case NonNegativeStockPolicy:
                    result = await policy.Validate(productDto.Stock.ToString(), Guid.Empty);
                    validationResult.Errors.AddRange(result.Errors);
                    break;
                case UniqueNamePolicy:
                    result = await policy.Validate(productDto.Name, productDto.Id);
                    validationResult.Errors.AddRange(result.Errors);
                    break;
                case PriceRangePolicy:
                    result = await policy.Validate(productDto.Price.ToString(), productDto.CategoryId);
                    validationResult.Errors.AddRange(result.Errors);
                    break;
                default:
                    result = await policy.Validate(productDto.Name, Guid.Empty);
                    validationResult.Errors.AddRange(result.Errors);
                    break;
            }
            
        }

        return validationResult;

    }

    public async Task<bool> ReserveProductAsync(Guid productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Nieprawidłowa liczba sztuk do rezerwacji.");

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new ArgumentException("Produkt nie istnieje.");

        if (product.AvailableStock < quantity)
            return false;
        ProductReservation productReservation = new ProductReservation()
        {
            ProductId = productId,
            Product =  product,
            ReservedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };
        await _productReservation.AddAsync(productReservation);
        product.ReservedStock += quantity;
        product.UpdatedAt = DateTime.UtcNow;
        
        await _productRepository.UpdateAsync(product);
        return true;
    }
    public async Task<bool> ReleaseProductAsync(Guid productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Nieprawidłowa liczba sztuk do rezerwacji.");

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new ArgumentException("Produkt nie istnieje.");

        if (product.ReservedStock < quantity)
            return false;

        product.ReservedStock -= quantity;
        product.UpdatedAt = DateTime.UtcNow;
        
        await _productRepository.UpdateAsync(product);
        return true;
    }

    
}