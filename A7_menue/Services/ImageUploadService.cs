using A7_menue.Configurations;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace A7_menue.Services;

public class ImageUploadService : IImageUploadService
{
    private readonly Cloudinary _cloudinary;

    public ImageUploadService(IOptions<CloudinarySettings> config)
    {
        var settings = config.Value;
        var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file was provided.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
            throw new ArgumentException("Only jpg, jpeg, png, and webp files are allowed.");

        if (file.Length > 5 * 1024 * 1024)
            throw new ArgumentException("Image size must not exceed 5MB.");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "menuea7-items",
            Transformation = new Transformation().Width(800).Height(800).Crop("limit")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new Exception($"Image upload failed: {result.Error.Message}");

        return result.SecureUrl.ToString();
    }

    public async Task DeleteImageAsync(string imageUrl)
    {
        var publicId = ExtractPublicId(imageUrl);
        if (string.IsNullOrEmpty(publicId)) return;

        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }

    private string? ExtractPublicId(string imageUrl)
    {

        try
        {
            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');

            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex == -1) return null;

            var relevantParts = segments.Skip(uploadIndex + 2);
            var publicIdWithExtension = string.Join("/", relevantParts);
            var publicId = Path.Combine(Path.GetDirectoryName(publicIdWithExtension) ?? "",
                Path.GetFileNameWithoutExtension(publicIdWithExtension)).Replace("\\", "/");

            return publicId;
        }
        catch
        {
            return null;
        }
    }
}