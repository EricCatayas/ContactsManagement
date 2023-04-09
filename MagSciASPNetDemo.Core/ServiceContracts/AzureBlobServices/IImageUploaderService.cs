using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.AzureBlobServices
{
    /// <summary>
    /// Defines a service for uploading images to Azure Blob Storage.
    /// </summary>
    public interface IImageUploaderService
    {
        /// <summary>
        /// Uploads an image to Azure Blob Storage.
        /// </summary>
        /// <param name="imageData">The binary data of the image to upload.</param>
        /// <param name="imageName">The name of the image file.</param>
        /// <returns>The URI of the uploaded image.</returns>
        Task<string> UploadImageAsync(byte[] imageData, string imageName);
    }
}
