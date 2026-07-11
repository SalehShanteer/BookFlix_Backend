namespace BookFlix.Core.Abstractions
{
    public interface IEntityFile : IEntity
    {
        string FileLocation { get; set; }
    }
}
