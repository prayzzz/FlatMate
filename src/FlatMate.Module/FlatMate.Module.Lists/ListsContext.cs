using FlatMate.Module.Lists.Models;
using Microsoft.EntityFrameworkCore;

namespace FlatMate.Module.Lists
{
    public class ListsContext : DbContext
    {
        public ListsContext(DbContextOptions<ListsContext> options)
            : base(options)
        {
        }

        public DbSet<ItemListDbo> List { get; set; }

        public DbSet<ItemDbo> ListEntry { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ItemListDbo>().ToTable("lists_list");
            builder.Entity<ItemDbo>().ToTable("lists_listentry");
        }
    }
}
