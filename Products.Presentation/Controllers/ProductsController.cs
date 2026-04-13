using Products.Library.Contracts.DTO;
using Products.Library.Contracts.Services;
using Products.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Products.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    /// <summary>Returns all products.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetAllAsync();
        return result.ToActionResult(this);
    }

    /// <summary>Returns a product by its ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _productService.GetByIdAsync(id);
        return result.ToActionResult(this);
    }

    /// <summary>Creates a new product.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var result = await _productService.CreateAsync(dto);

        if (!result.Success)
            return result.ToActionResult(this);

        return CreatedAtAction(nameof(GetById), new { id = result.Result!.Id }, result.Result);
    }

    /// <summary>Updates an existing product.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var result = await _productService.UpdateAsync(id, dto);
        return result.ToActionResult(this);
    }

    /// <summary>Deletes a product by ID.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);
        return result.ToActionResult(this);
    }
}
