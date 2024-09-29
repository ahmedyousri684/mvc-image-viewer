using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoaaFileViewer.Models
{
    public class ImageViewModel
    {
        public string DirectoryPath { get; set; }
        public List<ImageModel> Images { get; set; } = new List<ImageModel>();
        public string ErrorMessage { get; set; } = "";
    }
}
