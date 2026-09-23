using A7_menue.DTOs;

namespace A7_menue.Services;

public interface IPublicMenuService
{
    Task<PublicMenuResponse?> GetMenuBySlugAsync(string slug);
}