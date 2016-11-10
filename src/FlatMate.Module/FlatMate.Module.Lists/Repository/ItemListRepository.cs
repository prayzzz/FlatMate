using System.Linq;
using FlatMate.Common.Repository;
using FlatMate.Module.Lists.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlatMate.Module.Lists.Repository
{
    public class ItemListRepository : Repository<ItemListDbo>
    {
        private readonly ListsContext _context;
        private readonly ILogger<ItemListRepository> _logger;

        public ItemListRepository(ListsContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ItemListRepository>();
        }

        protected override DbContext Context => _context;

        protected override ILogger Logger => _logger;

        public override IQueryable<ItemListDbo> GetAll()
        {
            return _context.ItemListsFull;
        }
    }
}