using BookFlix.Core.Abstractions;

namespace BookFlix.Core.Models
{
    public class UploadedFile : BaseEntity
    {
        public string FileLocation { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
