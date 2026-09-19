namespace A7_menue.Models;
public class MenuCategory
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
