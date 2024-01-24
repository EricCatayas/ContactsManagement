
namespace ContactsManagement.Core.ServiceContracts.Others
{
    public interface IImageResizer
    {
        Stream Resize(Stream fileStream);
    }
}
