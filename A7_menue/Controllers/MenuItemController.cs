using A7_menue.DTOs;
using A7_menue.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace A7_menue.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MenuItemController(IMenuItemService menuItemService, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MenuItemResponse>>> GetAll()
    {
        var tenantId = currentUser.TenantId!.Value;
        return Ok(await menuItemService.GetAllAsync(tenantId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItemResponse>> GetById(int id)
    {
        var tenantId = currentUser.TenantId!.Value;
        var item = await menuItemService.GetByIdAsync(tenantId, id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<MenuItemResponse>> Create(CreateMenuItemRequest request)
    {
        var tenantId = currentUser.TenantId!.Value;
        var item = await menuItemService.CreateAsync(tenantId, request);
        if (item == null) return BadRequest("Invalid category.");
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MenuItemResponse>> Update(int id, UpdateMenuItemRequest request)
    {
        var tenantId = currentUser.TenantId!.Value;
        var item = await menuItemService.UpdateAsync(tenantId, id, request);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tenantId = currentUser.TenantId!.Value;
        var deleted = await menuItemService.DeleteAsync(tenantId, id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}/toggle-availability")]
    public async Task<IActionResult> ToggleAvailability(int id)
    {
        var tenantId = currentUser.TenantId!.Value;
        var toggled = await menuItemService.ToggleAvailabilityAsync(tenantId, id);
        if (!toggled) return NotFound();
        return NoContent();
    }
}