using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.Interfaces;

namespace ProductApp.API.Controllers;

[Controller]
[Route("api/v1/product-history")]
public class ProductChangeHistoryController: ControllerBase
{
    private readonly IProductChangeHistoryService _productChangeHistoryService;

    public ProductChangeHistoryController(IProductChangeHistoryService productChangeHistoryService)
    {
        _productChangeHistoryService = productChangeHistoryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var history = await _productChangeHistoryService.GetAllProductsHistoryAsync();
        return Ok(history);
    }
}