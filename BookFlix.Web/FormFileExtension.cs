using BookFlix.Core.Models;

namespace BookFlix.Web
{
    public static class FormFileExtension
    {
        public static FileUploadModel ToFileUploadModel(this IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return FileUploadModel.EmptyFile();
            }
            var stream = formFile.OpenReadStream();
            var fileName = formFile.FileName;
            var contentType = formFile.ContentType;
            long length = formFile.Length;

            return new FileUploadModel(stream, fileName, contentType, length);        
        }
    }
}
