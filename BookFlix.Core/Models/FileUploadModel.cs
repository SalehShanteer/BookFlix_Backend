namespace BookFlix.Core.Models
{
    public class FileUploadModel : IAsyncDisposable, IDisposable
    {
        public Stream Stream { get; init; }
        public string FileName { get; init; }
        public string ContentType { get; init; }
        public long Length { get; init; }
        public string Extension => Path.GetExtension(FileName);

        public FileUploadModel(Stream stream, string fileName, string contentType, long length)
        {
            Stream = stream ?? Stream.Null;
            FileName = fileName ?? string.Empty;
            ContentType = contentType ?? "application/octet-stream";
            Length = length;
        }

        public static FileUploadModel EmptyFile() => new (Stream.Null, string.Empty, string.Empty, 0);
       
        public void Dispose()
        {
            Stream?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (Stream is not null)
            {
                await Stream.DisposeAsync();
            }

            GC.SuppressFinalize(this);
        }
    }
}
