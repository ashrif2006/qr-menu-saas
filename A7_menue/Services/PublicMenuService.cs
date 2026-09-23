using A7_menue.Data;
using A7_menue.DTOs;
using Microsoft.EntityFrameworkCore;

namespace A7_menue.Services;
public class PublicMenuService(AppDbContext context) : IPublicMenuService
{
    public async Task<PublicMenuResponse?> GetMenuBySlugAsync(string slug)
    {
        var tenant = await context.Tenants
            .Where(t => t.Slug == slug && t.IsActive)
            .Select(t => new { t.Id, t.Name, t.LogoUrl })
            .FirstOrDefaultAsync();

        if (tenant == null) return null;

        var categories = await context.MenuCategories
            .Where(c => c.TenantId == tenant.Id && c.IsActive)
            .OrderBy(c => c.SortOrder)
            .Select(c => new PublicCategoryResponse
            {
                Id = c.Id,
                NameAr = c.Translations.Where(t => t.LanguageCode == "ar").Select(t => t.Name).FirstOrDefault() ?? "",
                NameEn = c.Translations.Where(t => t.LanguageCode == "en").Select(t => t.Name).FirstOrDefault() ?? "",
                Items = c.MenuItems
                    .Where(i => i.IsAvailable)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new PublicMenuItemResponse
                    {
                        Id = i.Id,
                        NameAr = i.Translations.Where(t => t.LanguageCode == "ar").Select(t => t.Name).FirstOrDefault() ?? "",
                        NameEn = i.Translations.Where(t => t.LanguageCode == "en").Select(t => t.Name).FirstOrDefault() ?? "",
                        DescriptionAr = i.Translations.Where(t => t.LanguageCode == "ar").Select(t => t.Description).FirstOrDefault(),
                        DescriptionEn = i.Translations.Where(t => t.LanguageCode == "en").Select(t => t.Description).FirstOrDefault(),
                        ImageUrl = i.ImageUrl,
                        Price = i.Price,
                        Variants = i.Variants
                            .Where(v => v.IsAvailable)
                            .OrderBy(v => v.SortOrder)
                            .Select(v => new PublicMenuItemVariantResponse
                            {
                                NameAr = v.Translations.Where(t => t.LanguageCode == "ar").Select(t => t.Name).FirstOrDefault() ?? "",
                                NameEn = v.Translations.Where(t => t.LanguageCode == "en").Select(t => t.Name).FirstOrDefault() ?? "",
                                Price = v.Price
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .ToListAsync();

        return new PublicMenuResponse
        {
            CafeName = tenant.Name,
            LogoUrl = tenant.LogoUrl,
            Categories = categories
        };
    }
}