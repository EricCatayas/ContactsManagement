using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using ContactsManagement.Core.ServiceContracts.Others.V2;
using MediaStorageServices.Interfaces.v2;
using ContactsManagement.Core.ServiceContracts.Others;
using Microsoft.AspNetCore.Http;

namespace ContactsManagement.Core.Services.Others.v2
{
    public class ImageResizeAndUploadService : IImageUploadService
    {
        private readonly IImageUploaderService _imageUploaderService;

        public ImageResizeAndUploadService(IImageUploaderService imageUploaderService)
        {
            _imageUploaderService = imageUploaderService;
        }

        public async Task<string> UploadAsync(IFormFile imageFile)
        {
            try
            {
                using (Stream stream = imageFile.OpenReadStream())
                {
                    var filename = imageFile.FileName;
                    var contentType = imageFile.ContentType;
                    return await _imageUploaderService.UploadAsync(stream, filename, contentType);
                }

            }
            catch (Exception ex)
            {
                // Handle the exception or log it as needed
                Console.WriteLine($"Error uploading image: {ex.Message}");
                throw;
            }
        }
    }
}
