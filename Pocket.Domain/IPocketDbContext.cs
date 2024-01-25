using Microsoft.EntityFrameworkCore;
using Pocket.Domain.Entities;

namespace Pocket.Domain;

public interface IPocketDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionGroup> TransactionGroups { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
