namespace BookFlix.Core.Repositories
{
    public interface IFileRepository
    {
        Task<bool> UpdateFileIDAsync(Guid id, Guid fileId);
    }
}
