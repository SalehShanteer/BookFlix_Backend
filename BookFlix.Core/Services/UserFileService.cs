using BookFlix.Core.Abstractions;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Services
{
    public class UserFileService : FileService<User>
    {
        public override string FolderName => "UserImages";

        public UserFileService(IFileStorageService fileStorageService, IUserRepository userRepository, IUploadedFileRepository uploadedFileRepository, ILogger<UserFileService> logger) : base(fileStorageService, userRepository, uploadedFileRepository, logger)
        {
        }

        public override Result ValidateFile(FileUploadModel file)
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
