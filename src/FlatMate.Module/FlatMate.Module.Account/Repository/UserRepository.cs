using System;
using System.Linq;
using FlatMate.Common.Attributes;
using FlatMate.Common.Repository;
using FlatMate.Module.Account.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FlatMate.Module.Account.Repository
{
    [Inject(DependencyLifetime.Scoped, typeof(IRepository<UserDbo>))]
    public class UserRepository : CachedRepository<UserDbo>
    {
        private readonly AccountContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AccountContext context, ILoggerFactory loggerFactory, IMemoryCache cache)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<UserRepository>();
            Cache = cache;
        }

        protected override IMemoryCache Cache { get; }

        protected override MemoryCacheEntryOptions CacheEntryOptions => new MemoryCacheEntryOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)};

        protected override DbContext Context => _context;

        protected override ILogger Logger => _logger;

        public override IQueryable<UserDbo> GetAll()
        {
            return _context.User;
        }
    }
}