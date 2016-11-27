using System.Linq;
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

        public DbSet<ItemListDbo> ItemLists { get; set; }

        public IQueryable<ItemListDbo> ItemListsFull => ItemLists.Include(x => x.Items)
                                                                 .Include(x => x.ListGroups).ThenInclude(x => x.Items);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("List");
        }
    }
}
