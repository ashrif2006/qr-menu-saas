using System.ComponentModel.DataAnnotations;

namespace A7_menue.DTOs;

public class CreateCategoryRequest
{
    public int SortOrder { get; set; }
    [Required(ErrorMessage ="Arabic name is requierd")]
    public string NameAr { get; set; } = string.Empty;
    [Required(ErrorMessage = "English name is requierd")]
    public string NameEn { get; set; } = string.Empty;
}


public class UpdateCategoryRequest
{
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
}


public class CategoryResponse
{
    public int Id { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
}