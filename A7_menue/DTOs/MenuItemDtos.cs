using System.ComponentModel.DataAnnotations;

namespace A7_menue.DTOs;

public class MenuItemVariantRequest
{
    public int SortOrder { get; set; }
    [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "NameAr is required.")]
    public string NameAr { get; set; } = string.Empty;
    [Required(ErrorMessage = "NameEn is required.")]
    public string NameEn { get; set; } = string.Empty;

}

public class CreateMenuItemRequest
{
    [Required(ErrorMessage = "CategoryId is required.")]
    public int CategoryId { get; set; }
    public int SortOrder { get; set; }
    public string? ImageUrl { get; set; }
    [Required(ErrorMessage = "NameAr is required.")]
    public string NameAr { get; set; } = string.Empty;
    [Required(ErrorMessage = "NameEn is required.")]
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    [Range(0.01,1000000, ErrorMessage = "Price must be greater than 0.")]
    public decimal? Price { get; set; }                   
    public List<MenuItemVariantRequest>? Variants { get; set; }
}

public class UpdateMenuItemRequest
{
    public int CategoryId { get; set; }
    public int SortOrder { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public decimal? Price { get; set; }
}
public class MenuItemVariantResponse
{
    public int Id { get; set; }
    public int SortOrder { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
}

public class MenuItemResponse
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int SortOrder { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public decimal? Price { get; set; }
    public List<MenuItemVariantResponse> Variants { get; set; } = new();
}
