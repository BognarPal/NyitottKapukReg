using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Jedlik.eDoc
{
    public class QRCode
    {
        public static string GenerateAsBase64(string content)
        {
            return $"data:image/png;base64,{Convert.ToBase64String(GenerateAsArray(content))}";
        }

        public static byte[] GenerateAsArray(string content)
        {
            using (var qrBitmap = GenerateAsBitmap(content))
                return qrBitmap.BitmapToByteArray();
        }

        public static Bitmap GenerateAsBitmap(string content)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeInfo = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCoder.QRCode(qrCodeInfo))
                return qrCode.GetGraphic(60);
        }

    }
}
