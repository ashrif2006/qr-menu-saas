namespace A7_menue.Models;
public class MenuItemVariantTranslation
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public string LanguageCode { get; set; } = string.Empty; // "ar" or "en"
    public string Name { get; set; } = string.Empty;         // "Small" / "صغير"

    // Navigation Property
    public MenuItemVariant Variant { get; set; } = null!;
}
