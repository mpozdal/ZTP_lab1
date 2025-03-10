using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.DTOs;
using ProductApp.Application.Interfaces;

namespace ProductApp.API.Controllers;

[Controller]
[Route("api/v1/forbidden-phrases")]
public class ForbiddenPhrasesController: ControllerBase
{
    private readonly IForbiddenPhraseService _forbiddenPhraseService;

    public ForbiddenPhrasesController(IForbiddenPhraseService forbiddenPhraseService)
    {
        _forbiddenPhraseService = forbiddenPhraseService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var phrases =  await _forbiddenPhraseService.GetAllPharsesAsync();
        return Ok(phrases);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var pharse = await _forbiddenPhraseService.GetPharseByIdAsync(id);
        if (pharse == null)
            return NoContent();
        return Ok(pharse);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ForbiddenPhraseDto forbiddenPhraseDto)
    {
        if (forbiddenPhraseDto == null)
        return BadRequest("bad request");
        
        var createdProduct = await _forbiddenPhraseService.CreatePharseAsync(forbiddenPhraseDto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _forbiddenPhraseService.DeletePharseAsync(id);
        if (!deleted)
        {
            return NotFound("not found");
        }

        return NoContent();
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ForbiddenPhraseDto forbiddenPhraseDto)
    {
        if (id != forbiddenPhraseDto.Id)
            return BadRequest("bad request");
        var updated = await _forbiddenPhraseService.UpdatePharseAsync(forbiddenPhraseDto);
        if (!updated)
        {
            return NotFound("not found");
        }

        return NoContent();
    }
    
}