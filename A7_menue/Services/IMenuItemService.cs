using A7_menue.DTOs;

namespace A7_menue.Services;

public interface IMenuItemService
{
    Task<List<MenuItemResponse>> GetAllAsync(int tenantId);
    Task<MenuItemResponse?> GetByIdAsync(int tenantId, int itemId);
    Task<MenuItemResponse?> CreateAsync(int tenantId, CreateMenuItemRequest request);
    Task<MenuItemResponse?> UpdateAsync(int tenantId, int itemId, UpdateMenuItemRequest request);
    Task<bool> DeleteAsync(int tenantId, int itemId);
    Task<bool> ToggleAvailabilityAsync(int tenantId, int itemId);

    Task<string?> UploadItemImageAsync(int tenantId, int itemId, IFormFile imageFile);
}

