using BookFlix.Core.Abstractions;

namespace BookFlix.Core.Models
{
    public abstract class BaseEntity : IEntity
    {
        public Guid ID { get; set; }
    }
}