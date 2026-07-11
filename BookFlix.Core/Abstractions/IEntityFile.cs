using BookFlix.Core.Models;

namespace BookFlix.Core.Abstractions
{
    public interface IEntityFile : IEntity
    {
        Guid? FileID { get; set; }
        UploadedFile File { get; set; }
    }
}
