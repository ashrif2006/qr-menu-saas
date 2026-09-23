using A7_menue.Services;
using Microsoft.AspNetCore.Mvc;

namespace A7_menue.Controllers;

[Route("api/public")]
[ApiController]
// لاحظ: مفيش [Authorize] هنا خالص، ده الفرق الأساسي
public class PublicController(IPublicMenuService publicMenuService) : ControllerBase
{
    [HttpGet("menu/{slug}")]
    public async Task<IActionResult> GetMenu(string slug)
    {
        var menu = await publicMenuService.GetMenuBySlugAsync(slug);

        if (menu == null) return NotFound("Cafe not found.");

        return Ok(menu);
    }
}