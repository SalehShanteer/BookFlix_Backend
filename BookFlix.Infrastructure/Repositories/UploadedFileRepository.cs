using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;

namespace BookFlix.Infrastructure.Repositories
{
    internal class UploadedFileRepository : EntityRepository<UploadedFile>, IUploadedFileRepository
    {
        public UploadedFileRepository(AppDbContext context) : base(context)
        {
        }
    }
}
