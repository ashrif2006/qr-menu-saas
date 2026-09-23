namespace A7_menue.Services;

public interface IQrCodeService
{
    byte[] GenerateQrCode(string slug);
}