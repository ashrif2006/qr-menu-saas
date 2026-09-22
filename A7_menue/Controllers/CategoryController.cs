using A7_menue.DTOs;
using A7_menue.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace A7_menue.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController 
    (ICategoryService categoryService , ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAll()
    {
        var tenantId = currentUser.TenantId!.Value;
        var categories = await categoryService.GetAllAsync(tenantId);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetById(int id)
    {
        var tenantId = currentUser.TenantId!.Value;
        var categry = await categoryService.GetById(tenantId, id);
        if(categry == null)
        {
            return NotFound();
        }
        return Ok(categry);
    }
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(CreateCategoryRequest request)
    {
        var tenantId = currentUser.TenantId!.Value;
        var category = await categoryService.createAsync(tenantId, request);
        return Ok(category);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponse>> Update(int id, UpdateCategoryRequest request)
    {
        var tenantId = currentUser.TenantId!.Value;
        var category = await categoryService.updateAsync(tenantId, id, request);

        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tenantId = currentUser.TenantId!.Value;
        var deleted = await categoryService.DeleteAsync(tenantId, id);

        if (!deleted) return NotFound();
        return NoContent();
    }
}

