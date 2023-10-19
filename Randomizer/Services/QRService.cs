using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Randomizer.Authorization;
using Randomizer.Entities;
using Randomizer.Exceptions;
using Randomizer.Models;


namespace Randomizer.Services
{
    public interface IQRService
    {
       

        public string QRGenerate(string productListUrl);
    }
    public class QRService : IQRService
    {
        
       

       

        public string QRGenerate(string productListUrl)
        {
            if(productListUrl == null) { throw new NotFoundException("Product list not found");}

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(productListUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            var qrCodeImageBytes = qrCode.GetGraphic(20);

            
            var qrCodeBase64 = Convert.ToBase64String(qrCodeImageBytes);

            return qrCodeBase64;

        }
       

    }
}
