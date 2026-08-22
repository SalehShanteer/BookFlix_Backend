using BookFlix.Core.Abstractions;
using BookFlix.Core.Logging;
using BookFlix.Core.Models;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Decorators
{
    public class LoggingFileService<T> : IFileService<T> where T : class, IEntityFile
    {
        private readonly ILogger<LoggingFileService<T>> _logger;
        private readonly IFileService<T> _inner;
        private static readonly string EntityTypeName = typeof(T).Name;

        public LoggingFileService(ILogger<LoggingFileService<T>> logger, IFileService<T> inner) 
        {
            _logger = logger;
            _inner = inner;
        }

        public async Task<Result<(Stream Stream, string ContentType)>> GetFileStreamAsync(Guid fileId)
        {
            var result = await _inner.GetFileStreamAsync(fileId);
            if (result.IsFailure)
            {
                _logger.LogGetFileStreamFailed(EntityTypeName, fileId, result.Error.Key);
            }
            return result;
        }

        public async Task<Result<Guid>> UploadFileAsync(Guid entityID, FileUploadModel file)
        {
            _logger.LogUploadFileExecuting(EntityTypeName, entityID);
            var result = await _inner.UploadFileAsync(entityID, file);

            if (result.IsFailure)
            {
                _logger.LogUploadFileFailed(EntityTypeName, entityID, result.Error.Key);
            }
            else
            {
                _logger.LogUploadFileSuccess(EntityTypeName, entityID, result.Value);
            }
            return result;
        }

        public Result ValidateFile(FileUploadModel file)
        {
            var result = _inner.ValidateFile(file);

            if (result.IsFailure)
            {
                _logger.LogFileValidationFailed(EntityTypeName, result.Error.Key);
            }
            return result;
        }
    }
}
