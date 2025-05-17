using ProductApp.API.Controllers;
using ProductApp.Application.DTOs;
using ProductApp.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Excetpions;
using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Validation;
using Xunit;

namespace ProductApp.Tests;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _categoryServiceMock = new Mock<ICategoryService>();
        _controller = new ProductController(_productServiceMock.Object);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenProductIsCreated()
    {
        Guid productId = new Guid();
        Guid categoryId = new Guid();
        // Arrange
        var productDto = new ProductDto
        {
            Id = productId,
            Name = "b",
            Description = "Test Description",
            CategoryId = categoryId,
            Price = 100,
            Stock = 1,
        };
        var categoryDto = new CategoryDto()
        {
            Id = categoryId,
            Name = "Books",
            MinPrice = 5,
            MaxPrice = 500
        };
        _categoryServiceMock.Setup(s => s.CreateCategoryAsync(It.IsAny<CategoryDto>())).ReturnsAsync(categoryDto);
        _productServiceMock.Setup(s => s.CreateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(productDto);

        // Act
        var result = await _controller.Create(productDto);
        Console.WriteLine("halo");
        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedProduct = Assert.IsType<ProductDto>(actionResult.Value);
        Assert.Equal(productDto.Id, returnedProduct.Id);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenProductDtoIsNull()
    {
        // Act
        var result = await _controller.Create(null);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Product data is required.", actionResult.Value);
    }
    [Fact]
    public async Task CreateProduct_ThrowsValidationException_WhenNameIsInvalid()
    {
        // Arrange
        var productDto = new ProductDto
        {
            Id = Guid.NewGuid(),
            Name = "a",
            Description = "Test",
            CategoryId = Guid.NewGuid(),
            Price = 50,
            Stock = 10
        };

        var validationErrors = new List<string> { "Product name is invalid" };
        var validationResult = new ValidationResult {  };

        var productServiceMock = new Mock<IProductService>();
        productServiceMock
            .Setup(s => s.CreateProductAsync(It.IsAny<ProductDto>()))
            .ThrowsAsync(new ValidationException(validationErrors));

        var controller = new ProductController(productServiceMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => controller.Create(productDto));
        Assert.Contains("Product name is invalid", exception.Errors);
    }

}

