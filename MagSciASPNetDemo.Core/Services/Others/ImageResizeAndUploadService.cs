
using MediaStorageServices.Interfaces;

namespace ContactsManagement.Core.Services.Others
{
    public class ImageResizeAndUploadService : IImageUploaderService
    {
        public Task<string> UploadAsync(byte[] imageData)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> UploadRangeAsync(IEnumerable<byte[]> imageDataList)
        {
            throw new NotImplementedException();
        }
    }
}
