namespace BookFlix.Core.Models
{
    public class UploadedFile : BaseEntity
    {
        public string FileLocation { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
