using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace BookFlix.Core.Services
{
    public class UserFileService : FileService<User>
    {
        public override string FolderName => "UserImages";

        public UserFileService(
            IUserRepository userRepository, 
            IUploadedFileRepository uploadedFileRepository, 
            ILogger<UserFileService> logger)
            : base(userRepository, uploadedFileRepository, logger)
        {
        }

        public override Result ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Result.Failure(Error.Failure("FileNullOrEmpty"));
            }

            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
            {
                return Result.Failure(Error.Failure("InvalidFileType"));
            }

            if (file.Length > 5 * 1024 * 1024) // 5MB limit
            {
                return Result.Failure(Error.Failure("FileSizeExceed5MB"));
            }

            return Result.Success();
        }
    }
}
