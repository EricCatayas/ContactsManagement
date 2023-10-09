
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using MediaStorageServices.Interfaces;
using MediaStorageServices.Services.AzureStorageContainer;
using Microsoft.Extensions.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ContactsManagement.Core.Services.Others
{
    public class ImageResizeAndUploadService : IImageUploaderService
    {
        private readonly IImageUploaderService _imageUploaderService;
        private const int IMAGE_WIDTH = 150;
        private const int IMAGE_HEIGHT = 150;
        public ImageResizeAndUploadService(IImageUploaderService imageUploaderService)
        {
            _imageUploaderService = imageUploaderService;
        }

        public async Task<string> UploadAsync(byte[] imageData)
        {
            try
            {
                using (var originalImageStream = new MemoryStream(imageData))
                using (var originalImage = Image.FromStream(originalImageStream))
                {
                    // Create a new Bitmap with the desired size
                    using (var resizedImage = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT))
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        // Draw the original image onto the resized image
                        graphics.DrawImage(originalImage, 0, 0, IMAGE_WIDTH, IMAGE_HEIGHT);

                        // Save the resized image to a new MemoryStream
                        using (var resizedImageStream = new MemoryStream())
                        {
                            var resizedImageBytes = resizedImageStream.ToArray();

                            return await _imageUploaderService.UploadAsync(resizedImageBytes);  
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public Task<IEnumerable<string>> UploadRangeAsync(IEnumerable<byte[]> imageDataList)
        {
            throw new NotImplementedException();
        }
    }
}
