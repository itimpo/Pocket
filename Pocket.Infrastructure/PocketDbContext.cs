using Microsoft.EntityFrameworkCore;
using Pocket.Domain;
using Pocket.Domain.Entities;

namespace Pocket.Infrastructure;

internal class PocketDbContext : DbContext, IPocketDbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<TransactionGroup> TransactionGroups { get; set; }

    public PocketDbContext(DbContextOptions<PocketDbContext> options)
       : base(options)
    {
        if (Database.EnsureCreated())
        {
            // Generate data for User
            Users?.AddRange(
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Dmytro",
                    Email = "dtymofieiev@gmail.com",
                    PasswordHash = "123123"
                }
            );

            // Generate data for Currency
            Currencies?.AddRange(
                new Currency { Id = 1, Code = "EUR" },
                new Currency { Id = 2, Code = "USD" }
            );

            SaveChanges();
        }
    }
}
