using ImageMagick;
using System.Drawing;
using System.IO;

namespace ShoaaFileViewer.Services
{
    public class PDFToImagesConverter
    {

        public void ConvertPdfToImages(Stream pdfStream, string targetDirectory)
        {
            List<Image> images = new List<Image>();

            // Define settings for converting the PDF to images
            MagickReadSettings settings = new MagickReadSettings()
            {
                Density = new Density(150, 150) // Higher density for better quality
            };

            // Read the PDF file into images
            using (var imagesCollection = new MagickImageCollection())
            {
                imagesCollection.Read(pdfStream, settings);

                // Convert each page of the PDF to an image
                foreach (var image in imagesCollection)
                {
                    image.Format = MagickFormat.Png; // Convert to JPG
                    image.Write(Path.Combine(targetDirectory, Guid.NewGuid() + ".png"));
                }
            }

        }
    }
}
