using Microsoft.AspNetCore.Http;

namespace Services
{
    public class ImageService
    {
        private readonly List<string> allowedFileTypes = new List<string>()
        {
            "png", "jpg", "jpeg", "webp", "gif"
        };

        public async Task<string> UploadFile(IFormFile formFile)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                var filePath = GetFilePath(fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetFilePath(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            filePath = Path.Combine(filePath, fileName);
            return filePath;
        }

        private void ValidateFileType(string extension)
        {
            if (!allowedFileTypes.Contains(extension.ToLower()))
            {
                throw new ArgumentException("This file type is not allowed.");
            }
        }
    }
}