using BookFlix.Core.Abstractions;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;

namespace BookFlix.Infrastructure.Repositories
{
    internal class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ITransaction> BeginTransactionAsync()
        {
            var dbTransaction = await _context.Database.BeginTransactionAsync();
            return new EfCoreTransaction(dbTransaction);
        }
    }
}
