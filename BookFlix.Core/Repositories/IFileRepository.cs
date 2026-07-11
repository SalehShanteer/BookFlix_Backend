namespace BookFlix.Core.Repositories
{
    public interface IFileRepository
    {
        Task<bool> UpdateFileLocationAsync(Guid id, string fileLocation);
    }
}
