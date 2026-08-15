using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;

namespace BookFlix.Infrastructure.Repositories
{
    internal class ReviewRepository : EntityRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context)
        {
        }
    }
}
