using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.AzureBlobServices
{
    /// <summary>
    /// Defines a service for deleting images from Azure Blob Storage.
    /// </summary>
    public interface IImageDeleterService
    {
        /// <summary>
        /// Deletes the blob file at the specified URL from Azure Blob Storage.
        /// </summary>
        /// <param name="azureBlobUrl">The URL of the blob file to delete.</param>
        Task<bool> DeleteBlobFile(string azureBlobUrl);
    }
}
