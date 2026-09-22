using A7_menue.DTOs;

namespace A7_menue.Services;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllAsync(int tenantId);
    Task<CategoryResponse> GetById(int tenantId , int categryId);
    Task<CategoryResponse> createAsync(int tenantId , CreateCategoryRequest request);
    Task<CategoryResponse> updateAsync(int tenantId, int categryId, UpdateCategoryRequest request);
    Task<bool> DeleteAsync(int tenantId , int categryId);
}

