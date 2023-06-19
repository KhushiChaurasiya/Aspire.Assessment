using Microsoft.EntityFrameworkCore;
using Assignment.Contracts.Data.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Assignment.Migrations
{
    [ExcludeFromCodeCoverage]
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries<BaseEntity>().AsEnumerable())
            {
                item.Entity.AddedOn = DateTime.Now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<App> App { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<AppDownload> AppDownload { get; set; }
        public DbSet<Logs> Logs { get; set; }
    }
}