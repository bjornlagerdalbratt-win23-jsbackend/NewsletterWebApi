using Microsoft.EntityFrameworkCore;
using NewsletterWebApi.Entities;

namespace NewsletterWebApi.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<SubscriberEntity> Subscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriberEntity>().ToContainer("Subscribers").HasPartitionKey(x => x.PartitionKey);
    }
}
