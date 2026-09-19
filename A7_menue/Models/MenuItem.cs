namespace A7_menue.Models;
public class MenuItem
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int CategoryId { get; set; }
    public decimal? Price { get; set; }       // nullable - null لو الصنف عنده Variants
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Tenant Tenant { get; set; } = null!;
    public MenuCategory Category { get; set; } = null!;
    public ICollection<MenuItemTranslation> Translations { get; set; } = new List<MenuItemTranslation>();
    public ICollection<MenuItemVariant> Variants { get; set; } = new List<MenuItemVariant>();
}
