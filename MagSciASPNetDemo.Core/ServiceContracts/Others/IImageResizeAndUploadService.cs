
namespace ContactsManagement.Core.ServiceContracts.Others
{
    public interface IImageResizeAndUploadService
    {
        Task<string> UploadAsync(byte[] imageData);
    }
}
