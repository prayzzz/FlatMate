using FlatMate.Module.Lists.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlatMate.Module.Lists.Persistence
{
    public class ListsContext : DbContext
    {
        public ListsContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ItemListGroupDbo> ItemListGroups { get; set; }

        public DbSet<ItemListDbo> ItemLists { get; set; }

        public DbSet<ItemDbo> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("List");

            builder.Entity<ItemListDbo>().HasMany(x => x.Groups).WithOne(x => x.List).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ItemListGroupDbo>().HasMany(x => x.Items).WithOne(x => x.Group).OnDelete(DeleteBehavior.Cascade);
        }
    }
}