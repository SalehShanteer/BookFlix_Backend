using BookFlix.Core.Abstractions;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Services
{
    public class BookFileService : FileService<Book>
    {
        public override string FolderName => "BookStorage";

        public BookFileService(IFileStorageService fileStorageService, IBookRepository bookRepository, IUploadedFileRepository uploadedFileRepository) : base(fileStorageService, bookRepository, uploadedFileRepository){}

        public override Result ValidateFile(FileUploadModel file)
        {
            if (file == null || file.Length == 0)
            {
                return Result.Failure(Error.Failure("FileNullOrEmpty"));
            }

            if (file.ContentType != "application/pdf")
            {
                return Result.Failure(Error.Failure("InvalidFileType"));
            }

            if (file.Length > 100 * 1024 * 1024)
            {
                return Result.Failure(Error.Failure("FileSizeExceed100MB"));
            }

            return Result.Success();
        }
    }
}
