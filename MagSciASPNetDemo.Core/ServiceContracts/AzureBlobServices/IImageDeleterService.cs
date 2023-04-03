using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.AzureBlobServices
{
    public interface IImageDeleterService
    {
        Task<bool> DeleteBlobFile(string azureBlobUrl);
    }
}
