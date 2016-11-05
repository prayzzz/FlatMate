using System;
using System.Linq;
using FlatMate.Module.Account.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FlatMate.Module.Account.Repository
{
    public class UserRepository
    {
        private readonly AccountContext _context;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public UserRepository(AccountContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;

            _cacheEntryOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) };
        }

        public UserDbo GetById(int id)
        {
            UserDbo user;
            if (_cache.TryGetValue(id, out user))
            {
                return user;
            }

            user = _context.User.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                _cache.Set(id, user, _cacheEntryOptions);
            }

            return user;
        }
    }
}