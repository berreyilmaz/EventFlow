using QRCoder;

namespace EventFlow.Services;

public class QrCodeService
{
    public byte[] Generate(string text)
    {
        using var generator = new QRCodeGenerator();

        using var data = generator.CreateQrCode(
            text,
            QRCodeGenerator.ECCLevel.Q);

        var qr = new PngByteQRCode(data);

        return qr.GetGraphic(20);
    }
}