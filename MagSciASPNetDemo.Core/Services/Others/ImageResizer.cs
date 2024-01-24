
using ContactsManagement.Core.ServiceContracts.Others;
using System.Drawing;

namespace ContactsManagement.Core.Services.Others
{
    // TODO: Verify
    public class ImageResizer : IImageResizer
    {
        private const int IMAGE_WIDTH = 150;
        private const int IMAGE_HEIGHT = 150;
        public Stream Resize(Stream fileStream)
        {
            using (var originalImage = Image.FromStream(fileStream))
            {
                if (originalImage.Width > IMAGE_WIDTH && originalImage.Height > IMAGE_HEIGHT)
                {
                    //Resize image
                    using (var resizedImage = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT))
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        // Draw the original image onto the resized image
                        graphics.DrawImage(originalImage, 0, 0, IMAGE_WIDTH, IMAGE_HEIGHT);

                        // Save the resized image to a memory stream
                        var resizedStream = new MemoryStream();
                        resizedImage.Save(resizedStream, originalImage.RawFormat);

                        // Reset the memory stream position to the beginning
                        resizedStream.Position = 0;

                        return resizedStream;
                    }
                }
                else
                {
                    // If the image is smaller than the target size, return the original stream
                    fileStream.Position = 0;
                    return fileStream;
                }

            }
        }
    }
}
