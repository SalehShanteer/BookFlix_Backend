namespace BookFlix.Core.Abstractions
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string prefix = "");
        Task<Stream> GetFileStreamAsync(string fileKey);
        Task<bool> DeleteFileAsync(string fileKey);
        Task<string> GetPresignedUrlAsync(string fileKey, TimeSpan expiry);
    }
}
