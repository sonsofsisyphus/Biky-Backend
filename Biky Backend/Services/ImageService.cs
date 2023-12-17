namespace Services
{
    public class ImageService
    {
        // List of allowed file extensions for uploaded images.
        private readonly List<string> allowedFileTypes = new List<string>()
        {
            ".png", ".jpg", ".jpeg", ".webp", ".gif"
        };

        // Method to upload a file and return the generated file name.
        public async Task<string> UploadFile(IFormFile formFile)
        {
            try
            {
                // Generate a unique file name based on a GUID and the original file's extension.
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                var filePath = GetFilePath(fileName);

                // Copy the uploaded file to the specified file path.
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

        // Helper method to get the full file path for storing uploaded images.
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

        // Helper method to validate if the file extension is allowed.
        private void ValidateFileType(string extension)
        {
            if (!allowedFileTypes.Contains(extension.ToLower()))
            {
                throw new ArgumentException("This file type is not allowed.");
            }
        }
        
    }
}