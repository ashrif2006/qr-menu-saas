namespace A7_menue.DTOs;

public class PublicMenuItemVariantResponse
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class PublicMenuItemResponse
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
    public List<PublicMenuItemVariantResponse> Variants { get; set; } = new();
}

public class PublicCategoryResponse
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public List<PublicMenuItemResponse> Items { get; set; } = new();
}

public class PublicMenuResponse
{
    public string CafeName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public List<PublicCategoryResponse> Categories { get; set; } = new();
}