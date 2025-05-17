using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.DTOs;
using ProductApp.Application.Services;
using ProductApp.Application.Interfaces;
namespace ProductApp.API.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productSerivce;
    public ProductController(IProductService productService)
    {
        _productSerivce = productService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var products = await _productSerivce.GetAllProductsAsync();
        return Ok(products);
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var product = await _productSerivce.GetProductByIdAsync(id);
        if (product == null)
            return NotFound("Not found");
        return Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductDto productDto)
    {
        if (productDto == null)
            return BadRequest("Product data is required.");
        var createdProduct = await _productSerivce.CreateProductAsync(productDto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _productSerivce.DeleteProductAsync(id);
        if (!deleted)
        {
            return NotFound("not found");
        }

        return NoContent();
    }
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] ProductDto productDto)
    {
        if (id != productDto.Id)
            return BadRequest("ID in the URL does not match the request body.");
        var updated = await _productSerivce.UpdateProductAsync(productDto);
        if (!updated)
        {
            return NotFound($"Product with ID {id} not found.");
        }

        return NoContent();
    }
    
    [HttpPost("{id:guid}/reserve")]
    public async Task<IActionResult> Reserve(Guid id, [FromQuery] int quantity)
    {
        var result = await _productSerivce.ReserveProductAsync(id, quantity);

        if (!result)
            return BadRequest("Nie ma wystarczającej liczby sztuk do rezerwacji.");

        return Ok($"Zarezerwowano {quantity} szt.");
    }
    [HttpPost("{id:guid}/release")]
    public async Task<IActionResult> Release(Guid id, [FromQuery] int quantity)
    {
        var result = await _productSerivce.ReleaseProductAsync(id, quantity);

        if (!result)
            return BadRequest("Nie ma wystarczającej liczby sztuk do rezerwacji.");

        return Ok($"Zwolniono {quantity} szt.");
    }
  
    
}