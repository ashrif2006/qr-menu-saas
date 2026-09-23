namespace A7_menue.Services;

public interface IImageUploadService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(string imageUrl);
}