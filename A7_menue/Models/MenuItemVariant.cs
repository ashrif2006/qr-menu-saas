namespace A7_menue.Models;
public class MenuItemVariant
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public decimal Price { get; set; }
    public int SortOrder { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Navigation Properties
    public MenuItem MenuItem { get; set; } = null!;
    public ICollection<MenuItemVariantTranslation> Translations { get; set; } = new List<MenuItemVariantTranslation>();
}