using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ImageService
    {
       private readonly List<String> allowedFileTypes = new List<String>()
       {
           "png", "jpg", "jpeg", "webp", "gif"
       };
        public async Task<string> UploadFile(IFormFile _IFormFile)
        {
            string FileName = "";
            try
            {
                FileInfo FileInfo = new FileInfo(_IFormFile.FileName);
                var filePath = GetFileDirectory(FileInfo.Extension); 
                using (var _FileStream = new FileStream(filePath, FileMode.Create))
                {
                    await _IFormFile.CopyToAsync(_FileStream);
                }
                return FileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetFileDirectory(String extension)
        {
            ValidateFileType(extension);
            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = Path.Combine(filePath, fileName);
            return filePath;
        }

        private bool ValidateFileType(string extension)
        {
            if(!allowedFileTypes.Contains(extension))
            {
                throw new ArgumentException("This file type is not allowed.");
            }
            return true;
        }

    }
}
