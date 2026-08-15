using Amazon.S3;
using Amazon.S3.Model;
using BookFlix.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace BookFlix.Infrastructure.Storage
{
    internal class S3FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3FileStorageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["S3:BucketName"];
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string prefix = "")
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileKey = string.IsNullOrEmpty(prefix) ? $"{Guid.NewGuid()}{fileExtension}" : $"{prefix}/{Guid.NewGuid()}{fileExtension}";

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = fileStream,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(putRequest);
            return fileKey;
        }

        public async Task<Stream> GetFileStreamAsync(string fileKey)
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey
            };

            var response = await _s3Client.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }

        public async Task<bool> DeleteFileAsync(string fileKey)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey
            };

            var response = await _s3Client.DeleteObjectAsync(deleteRequest);
            return response.HttpStatusCode == HttpStatusCode.NoContent ||
                   response.HttpStatusCode == HttpStatusCode.OK;
        }

        public Task<string> GetPresignedUrlAsync(string fileKey, TimeSpan expiry)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.Add(expiry)
            };

            var url = _s3Client.GetPreSignedURL(request);
            return Task.FromResult(url);
        }
    }
}