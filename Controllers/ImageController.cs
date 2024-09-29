using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShoaaFileViewer.Models;
using System.Reflection;

namespace ShoaaFileViewer.Controllers
{
    public class ImageController : Controller
    {
        private readonly MyAppSettings _mySettings;

        public ImageController(IOptions<MyAppSettings> mySettings)
        {
            _mySettings = mySettings.Value;
        }
        public IActionResult Index()
        {
            return View(new ImageViewModel());
        }

        [HttpGet("image/{folderName}/{nationaId}")]
        public IActionResult Index(string folderName, string nationaId)
        {
            var baseDiirectory = _mySettings.BaseDirectory;
                var pathName = baseDiirectory + "\\" + folderName + "\\" + nationaId;
                var model = new ImageViewModel();
            model.DirectoryPath = pathName;
            if (Directory.Exists(model.DirectoryPath))
            {
                var imageFiles = Directory.GetFiles(model.DirectoryPath)
                    .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg") || file.EndsWith(".gif") || file.EndsWith(".abr"))
                    .Select(file => new ImageModel
                    {
                        FileName = Path.GetFileName(file),
                        FilePath = file  // Store the full path to the file
                    }).ToList();
                // \\192.168.11.211\FileScan\FileScan\1\30001010130876 

                model.Images = imageFiles;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Directory does not exist.");
            }

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult ViewImages(ImageViewModel model)
        {
            if (Directory.Exists(model.DirectoryPath))
            {
                var imageFiles = Directory.GetFiles(model.DirectoryPath)
                    .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg") || file.EndsWith(".gif") || file.EndsWith(".abr"))
                    .Select(file => new ImageModel
                    {
                        FileName = Path.GetFileName(file),
                        FilePath = file  // Store the full path to the file
                    }).ToList();

                model.Images = imageFiles;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Directory does not exist.");
            }

            return View("Index", model);
        }

        public IActionResult GetImage(string filePath, string? fileName)
        {
            if (System.IO.File.Exists(filePath))
            {
                var image = System.IO.File.OpenRead(filePath);
                return File(image, "application/x-photoshop",fileName);  // Adjust the content type as necessary
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(string folder, List<IFormFile> images)
        {
            if (string.IsNullOrEmpty(folder))
            {
                ModelState.AddModelError("folder", "Folder name is required.");
                return View();
            }

            if (images == null || images.Count == 0)
            {
                ModelState.AddModelError("images", "Please select at least one image.");
                return View();
            }

            // Base directory from appsettings.json
            var baseDirectory = _mySettings.BaseDirectory;

            // Combine base directory and the user-selected folder
            var targetDirectory = folder;

            // Create the directory if it doesn't exist
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // Allowed file extensions (including .abr for Adobe Brushes)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".abr", ".svg", ".tiff", ".webp" };

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

                    // Validate extension
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("images", "Invalid file format. Allowed formats: .jpg, .png, .gif, .bmp, .abr, .svg, .tiff, .webp.");
                        return View();
                    }

                    var filePath = Path.Combine(targetDirectory, image.FileName);

                    // Save the image
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
            }
            var splitedPath = folder.Split('\\');
            var folderName = splitedPath[^2];
            var nationalId = splitedPath.Last();
            return Redirect("/Image/" + folderName + "/" + nationalId);
        }
    }
}
