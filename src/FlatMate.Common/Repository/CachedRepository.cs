using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Dbo;
using prayzzz.Common.Result;

namespace FlatMate.Common.Repository
{
    public abstract class CachedRepository<T> : Repository<T> where T : BaseDbo
    {
        protected abstract IMemoryCache Cache { get; }

        protected abstract MemoryCacheEntryOptions CacheEntryOptions { get; }

        public override Result<T> GetById(int id)
        {
            T cachedDbo;
            if (Cache.TryGetValue(id, out cachedDbo))
            {
                Logger.LogInformation("Get {0} with id {1} from cache", EntityName, id);
                return new SuccessResult<T>(cachedDbo);
            }

            var result = base.GetById(id);

            if (result.IsSuccess)
            {
                Cache.Set(id, result.Data, CacheEntryOptions);
            }

            return result;
        }
    }
}