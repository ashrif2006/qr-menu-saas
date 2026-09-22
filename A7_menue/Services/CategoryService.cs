using A7_menue.Data;
using A7_menue.DTOs;
using A7_menue.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace A7_menue.Services
{
    public class CategoryService(AppDbContext context) : ICategoryService
    {
        public async Task<CategoryResponse> createAsync(int tenantId, CreateCategoryRequest request)
        {
            var category = new MenuCategory
            {
                TenantId = tenantId,
                SortOrder = request.SortOrder,
                IsActive = true,
                Translations = new List<CategoryTranslation>
                {
                    new() { LanguageCode = "ar", Name = request.NameAr },
                    new() { LanguageCode = "en", Name = request.NameEn }
                }
            };
            context.MenuCategories.Add(category);
            await context.SaveChangesAsync();
            return MapToResponse(category);
        }

        public async Task<bool> DeleteAsync(int tenantId, int categryId)
        {
            var categroy = await context.MenuCategories
                .FirstOrDefaultAsync(c => c.Id == categryId);
            if (categroy == null) return false;
            context.MenuCategories.Remove(categroy);
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<List<CategoryResponse>?> GetAllAsync(int tenantId)
        {
            return await context.MenuCategories
                .Where(category => category.TenantId == tenantId)
                .Include(c => c.Translations)
                .OrderBy(c => c.SortOrder)
                .Select(c => MapToResponse(c))
                .ToListAsync();

        }

        public async Task<CategoryResponse?> GetById(int tenantId, int categryId)
        {
            var category = await context.MenuCategories
            .Include(c => c.Translations)
            .FirstOrDefaultAsync(c => c.Id == categryId && c.TenantId == tenantId);

            return category== null? null : MapToResponse(category);

        }

        public async Task<CategoryResponse?> updateAsync(int tenantId, int categryId, UpdateCategoryRequest request)
        {
            var category = await context.MenuCategories
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c =>c.Id == categryId && c.TenantId==tenantId);
            if(category == null)
            {
                return null;
            }
            category.SortOrder = request.SortOrder;
            category.IsActive = request.IsActive;

            var arTranslation = category.Translations.FirstOrDefault(t => t.LanguageCode == "ar");
            if (arTranslation != null) arTranslation.Name = request.NameAr;

            var enTranslation = category.Translations.FirstOrDefault(t => t.LanguageCode == "en");
            if (enTranslation != null) enTranslation.Name = request.NameEn;
            await context.SaveChangesAsync();
            return MapToResponse(category);

        }

        private static CategoryResponse MapToResponse(MenuCategory category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                SortOrder = category.SortOrder,
                IsActive = category.IsActive,
                NameAr = category.Translations.FirstOrDefault(t => t.LanguageCode == "ar")?.Name ?? "",
                NameEn = category.Translations.FirstOrDefault(t => t.LanguageCode == "en")?.Name ?? ""
            };
        }
    }
}
