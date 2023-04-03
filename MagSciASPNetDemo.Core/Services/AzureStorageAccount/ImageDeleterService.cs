using Azure.Storage.Blobs;
using ContactsManagement.Core.ServiceContracts.AzureBlobServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.AzureStorageAccount
{
    public class ImageDeleterService : IImageDeleterService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public ImageDeleterService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StorageAccountConnectionString");
            _containerName = config["BlobContainerName"].ToString();
        }
        public async Task<bool> DeleteBlobFile(string azureBlobUrl)
        {
            var blobUrl = new Uri(azureBlobUrl);
            var blobName = blobUrl.Segments[blobUrl.Segments.Length - 1];

            var blobServiceClient = new BlobServiceClient(_connectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Get a reference to the blob file using its name or URL
            var blobClient = containerClient.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
