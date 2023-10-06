using Azure.Storage.Blobs;
using ContactsManagement.Core.ServiceContracts.AzureBlobServices;
using Microsoft.Extensions.Configuration;
using System.Drawing.Imaging;
using System.Drawing;

namespace ContactsManagement.Core.Services.AzureStorageAccount
{
    public class ImageUploaderService : IImageUploaderService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private const int IMAGE_WIDTH = 150;
        private const int IMAGE_HEIGHT = 150;

        public ImageUploaderService(IConfiguration config)
        {
            _connectionString = config["StorageAccountConnectionString"].ToString();
            _containerName = config["BlobContainerName"].ToString();
        }
        public async Task<string> UploadImageAsync(byte[] imageData, string imageName)
        {
            try
            {
                imageName = imageName.Replace(' ', '_');
                BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
           
                BlobClient blobClient = containerClient.GetBlobClient(imageName);

                using (var imageStream = new MemoryStream(imageData))
                {
                    using (var image = System.Drawing.Image.FromStream(imageStream))
                    {

                        using (var bitmap = new Bitmap(image, new Size(IMAGE_WIDTH, IMAGE_HEIGHT)))
                        {
                            // Save the resized image to a memory stream
                            using (var resizedImageStream = new MemoryStream())
                            {
                                bitmap.Save(resizedImageStream, ImageFormat.Jpeg);

                                // Upload the resized image to Azure Blob Storage
                                resizedImageStream.Position = 0;
                                await blobClient.UploadAsync(resizedImageStream, overwrite: false);
                            }
                        }
                    }
                }
                    return blobClient.Uri.ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}
