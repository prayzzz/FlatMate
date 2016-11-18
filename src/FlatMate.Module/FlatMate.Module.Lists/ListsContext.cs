using System.Linq;
using FlatMate.Module.Lists.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlatMate.Module.Lists
{
    public class ListsContext : DbContext
    {
        public ListsContext(DbContextOptions<ListsContext> options)
            : base(options)
        {
        }

        public DbSet<ItemListDbo> ItemLists { get; set; }

        public IQueryable<ItemListDbo> ItemListsFull => ItemLists.Include(x => x.Items)
                                                                 .Include(x => x.ListGroups).ThenInclude(x => x.Items);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("List");

            //builder.Entity<ItemListDbo>().ToTable("Lists_ItemList");
            //builder.Entity<ItemListDbo>().HasMany(x => x.Items).WithOne(x => x.ItemList).HasForeignKey(x => x.ItemListId).OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<ItemListDbo>().HasMany(x => x.ListGroups).WithOne(x => x.ItemList).HasForeignKey(x => x.ItemListId).OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<ItemListGroupDbo>().ToTable("Lists_ItemListGroup");
            //builder.Entity<ItemListGroupDbo>().HasMany(x => x.Items).WithOne(x => x.ItemListGroup).HasForeignKey(x => x.ItemListGroupId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
