using Azure.Storage.Blobs;
using ContactsManagement.Core.ServiceContracts.AzureBlobServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.AzureStorageAccount
{
    public class ImageUploaderService : IImageUploaderService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly int _imageSize = 150;

        public ImageUploaderService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StorageAccountConnectionString");
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
                        int newHeight = (int)(((double)image.Height / (double)image.Width) * _imageSize);

                        using (var bitmap = new Bitmap(image, new Size(_imageSize, newHeight)))
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
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
