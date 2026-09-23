using A7_menue.Data;
using A7_menue.DTOs;
using A7_menue.Models;
using Microsoft.EntityFrameworkCore;

namespace A7_menue.Services;

public class MenuItemService(AppDbContext context , IImageUploadService imageUploadService) : IMenuItemService
{
    public async Task<List<MenuItemResponse>> GetAllAsync(int tenantId)
    {
        return await context.MenuItems
            .Where(i => i.TenantId == tenantId)
            .OrderBy(i => i.SortOrder)
            .Select(i => new MenuItemResponse
            {
                Id = i.Id,
                CategoryId = i.CategoryId,
                SortOrder = i.SortOrder,
                IsAvailable = i.IsAvailable,
                ImageUrl = i.ImageUrl,
                Price = i.Price,
                NameAr = i.Translations.FirstOrDefault(t => t.LanguageCode == "ar")!.Name,
                NameEn = i.Translations.FirstOrDefault(t => t.LanguageCode == "en")!.Name,
                DescriptionAr = i.Translations.FirstOrDefault(t => t.LanguageCode == "ar")!.Description,
                DescriptionEn = i.Translations.FirstOrDefault(t => t.LanguageCode == "en")!.Description,
                Variants = i.Variants.Select(v => new MenuItemVariantResponse
                {
                    Id = v.Id,
                    SortOrder = v.SortOrder,
                    Price = v.Price,
                    IsAvailable = v.IsAvailable,
                    NameAr = v.Translations.FirstOrDefault(t => t.LanguageCode == "ar")!.Name,
                    NameEn = v.Translations.FirstOrDefault(t => t.LanguageCode == "en")!.Name
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<MenuItemResponse?> GetByIdAsync(int tenantId, int itemId)
    {
        return await context.MenuItems
            .Where(i => i.Id == itemId && i.TenantId == tenantId)
            .Select(i => new MenuItemResponse
            {
                Id = i.Id,
                CategoryId = i.CategoryId,
                SortOrder = i.SortOrder,
                IsAvailable = i.IsAvailable,
                ImageUrl = i.ImageUrl,
                Price = i.Price,
                NameAr = i.Translations.FirstOrDefault(t => t.LanguageCode == "ar")!.Name,
                NameEn = i.Translations.FirstOrDefault(t => t.LanguageCode == "en")!.Name,
                DescriptionAr = i.Translations.FirstOrDefault(t => t.LanguageCode == "ar")!.Description,
                DescriptionEn = i.Translations.FirstOrDefault(t => t.LanguageCode == "en")!.Description,
                Variants = i.Variants.Select(v => new MenuItemVariantResponse
                {
                    Id = v.Id,
                    SortOrder = v.SortOrder,
                    Price = v.Price,
                    IsAvailable = v.IsAvailable,
                    NameAr = v.Translations.FirstOrDefault(t => t.LanguageCode == "ar")!.Name,
                    NameEn = v.Translations.FirstOrDefault(t => t.LanguageCode == "en")!.Name
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    // ===== WRITE: لازم Include عشان بنبني Entity هنضيفه =====
    public async Task<MenuItemResponse?> CreateAsync(int tenantId, CreateMenuItemRequest request)
    {
        var categoryExists = await context.MenuCategories
            .AnyAsync(c => c.Id == request.CategoryId && c.TenantId == tenantId);

        if (!categoryExists) return null;

        var item = new MenuItem
        {
            TenantId = tenantId,
            CategoryId = request.CategoryId,
            SortOrder = request.SortOrder,
            ImageUrl = request.ImageUrl,
            IsAvailable = true,
            Price = request.Price,
            Translations = new List<MenuItemTranslation>
            {
                new() { LanguageCode = "ar", Name = request.NameAr, Description = request.DescriptionAr },
                new() { LanguageCode = "en", Name = request.NameEn, Description = request.DescriptionEn }
            }
        };

        if (request.Variants != null && request.Variants.Count > 0)
        {
            foreach (var v in request.Variants)
            {
                item.Variants.Add(new MenuItemVariant
                {
                    SortOrder = v.SortOrder,
                    Price = v.Price,
                    IsAvailable = true,
                    Translations = new List<MenuItemVariantTranslation>
                    {
                        new() { LanguageCode = "ar", Name = v.NameAr },
                        new() { LanguageCode = "en", Name = v.NameEn }
                    }
                });
            }
        }

        context.MenuItems.Add(item);
        await context.SaveChangesAsync();

        return await GetByIdAsync(tenantId, item.Id);
    }

    public async Task<MenuItemResponse?> UpdateAsync(int tenantId, int itemId, UpdateMenuItemRequest request)
    {
        var item = await context.MenuItems
            .Include(i => i.Translations)
            .FirstOrDefaultAsync(i => i.Id == itemId && i.TenantId == tenantId);

        if (item == null) return null;

        var categoryExists = await context.MenuCategories
            .AnyAsync(c => c.Id == request.CategoryId && c.TenantId == tenantId);
        if (!categoryExists) return null;

        item.CategoryId = request.CategoryId;
        item.SortOrder = request.SortOrder;
        item.IsAvailable = request.IsAvailable;
        item.ImageUrl = request.ImageUrl;
        item.Price = request.Price;

        var arT = item.Translations.FirstOrDefault(t => t.LanguageCode == "ar");
        if (arT != null) { arT.Name = request.NameAr; arT.Description = request.DescriptionAr; }

        var enT = item.Translations.FirstOrDefault(t => t.LanguageCode == "en");
        if (enT != null) { enT.Name = request.NameEn; enT.Description = request.DescriptionEn; }

        await context.SaveChangesAsync();

        return await GetByIdAsync(tenantId, item.Id);
    }

    public async Task<bool> DeleteAsync(int tenantId, int itemId)
    {
        var item = await context.MenuItems
            .FirstOrDefaultAsync(i => i.Id == itemId && i.TenantId == tenantId);

        if (item == null) return false;

        context.MenuItems.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleAvailabilityAsync(int tenantId, int itemId)
    {
        var item = await context.MenuItems
            .FirstOrDefaultAsync(i => i.Id == itemId && i.TenantId == tenantId);

        if (item == null) return false;

        item.IsAvailable = !item.IsAvailable;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> UploadItemImageAsync(int tenantId, int itemId, IFormFile imageFile)
    {
        var item = await context.MenuItems.FirstOrDefaultAsync(i => i.Id == itemId && i.TenantId == tenantId);
        if (item == null) return null;

        if(!string.IsNullOrEmpty(item.ImageUrl))
        {
            await imageUploadService.DeleteImageAsync(item.ImageUrl);
        }
        var newImageUrl = await imageUploadService.UploadImageAsync(imageFile);
        item.ImageUrl = newImageUrl;
        await context.SaveChangesAsync();
        return newImageUrl;
    }
}