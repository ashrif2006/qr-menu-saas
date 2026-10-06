using QRCoder;

namespace A7_menue.Services;

public class QrCodeService(IConfiguration configuration) : IQrCodeService
{
    public byte[] GenerateQrCode(string slug)
    {
        var baseUrl = configuration["PublicMenuBaseUrl"];
        var menuUrl = $"{baseUrl}/{slug}";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(menuUrl, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);

        return qrCode.GetGraphic(20);
    }
}