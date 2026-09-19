namespace A7_menue.Models;
public class MenuItemTranslation
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public string LanguageCode { get; set; } = string.Empty; // "ar" or "en"
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation Property
    public MenuItem MenuItem { get; set; } = null!;
}