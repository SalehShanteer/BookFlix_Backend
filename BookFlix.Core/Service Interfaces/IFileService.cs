using BookFlix.Core.Abstractions;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IFileService<T> where T : class, IEntityFile
    {
        Task<Result<Guid>> UploadFileAsync(Guid entityID, FileUploadModel file);
        Result ValidateFile(FileUploadModel file);
        Task<Result<string>> GetFilePathAsync(Guid fileId);
    }

    public abstract class FileService<T> : IFileService<T> where T : class, IEntityFile
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IEntityfileRepository<T> _repository;
        private readonly IUploadedFileRepository _uploadedFileRepository;
        private readonly ILogger<FileService<T>> _logger; // should be removed and use decorator pattern instead
        private string _directory;

        public abstract string FolderName { get; }

        protected FileService(IFileStorageService fileStorageService, IEntityfileRepository<T> repository, IUploadedFileRepository uploadedFileRepository, ILogger<FileService<T>> logger)
        {
            _fileStorageService = fileStorageService;
            _repository = repository;
            _uploadedFileRepository = uploadedFileRepository;
            _logger = logger;
        }

        public abstract Result ValidateFile(FileUploadModel file);

        public async Task<Result<Guid>> UploadFileAsync(Guid entityID, FileUploadModel file)
        {
            var result = ValidateFile(file);
            if (result.IsFailure) return Result.Failure<Guid>(result.Error);

            var entity = await _repository.GetByIDAsync(entityID);
            if (entity is null)
            {
                _logger.LogError("{EntityType} with ID {EntityID} not found.", typeof(T).Name, entityID);
                return Result.Failure<Guid>(Error.NotFound($"{typeof(T).Name}NotFound"));
            }

            string filePath = null;
            UploadedFile uploadedFile = null;

            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                if (entity.FileID.HasValue)
                {
                    uploadedFile = await _uploadedFileRepository.GetByIDAsync(entity.FileID.Value);
                    if (uploadedFile is not null)
                    {
                        var isDeleted = await _fileStorageService.DeleteFileAsync(uploadedFile.FileLocation);
                       
                        if (!isDeleted) return Result.Failure<Guid>(Error.Failure($"Failed to delete file With key {uploadedFile.FileLocation}"));
                    }
                }

                filePath = await _fileStorageService.UploadFileAsync(file.Stream, file.FileName, file.ContentType);
        
                if (uploadedFile is null)
                {
                    uploadedFile = new UploadedFile
                    {
                        ID = Guid.NewGuid(),
                        FileLocation = filePath,
                        ContentType = file.ContentType,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _uploadedFileRepository.AddAsync(uploadedFile);
                }
                else
                {
                    uploadedFile.FileLocation = filePath;
                    uploadedFile.ContentType = file.ContentType;
                    uploadedFile.UpdatedAt = DateTime.UtcNow;
                }

                await _repository.UpdateFileIDAsync(entityID, uploadedFile.ID);
                await _repository.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result.Success(uploadedFile.ID);
            }
            catch (IOException ex)
            {
                await transaction.RollbackAsync();
                await _fileStorageService.DeleteFileAsync(filePath);
                _logger.LogError(ex, "IO error uploading file for {EntityType} ID {EntityID}", typeof(T).Name, entityID);
                return Result.Failure<Guid>(Error.Failure("FileSaveStorageError"));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected error uploading file for {EntityType} ID {EntityID}", typeof(T).Name, entityID);
                return Result.Failure<Guid>(Error.Failure("FileUnexpectedUploadError"));
            }
        }

        public async Task<Result<string>> GetFilePathAsync(Guid fileId)
        {
            var uploadedFile = await _uploadedFileRepository.GetByIDAsync(fileId);
            if (uploadedFile is null)
            {
                return Result.Failure<string>(Error.NotFound("FileNotFound"));
            }

            var filePath = Path.Combine(_directory, uploadedFile.FileLocation);
            if (!File.Exists(filePath))
            {
                return Result.Failure<string>(Error.NotFound("FileNotFound"));
            }

            return Result.Success(filePath);
        }
    }
}
