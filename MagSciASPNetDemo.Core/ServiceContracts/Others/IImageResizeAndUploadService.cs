
using Microsoft.AspNetCore.Http;

namespace ContactsManagement.Core.ServiceContracts.Others
{
    public interface IImageResizeAndUploadService
    {
        Task<string> UploadAsync(byte[] imageData);
    }
}
namespace ContactsManagement.Core.ServiceContracts.Others.V2
{
    
    public interface IImageUploadService
    {
        Task<string> UploadAsync(IFormFile imageFile);
    }
}
