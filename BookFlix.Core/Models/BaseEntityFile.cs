using BookFlix.Core.Abstractions;

namespace BookFlix.Core.Models
{
    public abstract class BaseEntityFile : IEntityFile
    {
        public Guid ID { get; set; }
        public string FileLocation { get; set; }
    }
}
