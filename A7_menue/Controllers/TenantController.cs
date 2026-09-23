using A7_menue.Data;
using A7_menue.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace A7_menue.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TenantController(AppDbContext context, IQrCodeService qrCodeService, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet("qr-code")]
    public async Task<IActionResult> GetQrCode()
    {
        var tenantId = currentUser.TenantId!.Value;

        var slug = await context.Tenants
            .Where(t => t.Id == tenantId)
            .Select(t => t.Slug)
            .FirstOrDefaultAsync();

        if (slug == null) return NotFound("Tenant not found.");

        var qrBytes = qrCodeService.GenerateQrCode(slug);

        return File(qrBytes, "image/png", $"menu-qr-{slug}.png");
    }
}