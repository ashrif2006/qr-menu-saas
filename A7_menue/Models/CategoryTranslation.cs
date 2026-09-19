namespace A7_menue.Models;
public class CategoryTranslation
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string LanguageCode { get; set; } = string.Empty; // "ar" or "en"
    public string Name { get; set; } = string.Empty;

    // Navigation Property
    public MenuCategory Category { get; set; } = null!;
}
