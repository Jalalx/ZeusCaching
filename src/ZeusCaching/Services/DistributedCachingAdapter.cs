using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZeusCaching.Services
{
    public class DistributedCachingAdapter : ICachingAdapter
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCachingAdapter(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public Task<string> GetContentAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            return _distributedCache.GetStringAsync(key, cancellationToken);
        }

        public Task SetContentAsync(
            string key,
            string content,
            TimeSpan? slidingExpiration = null,
            DateTimeOffset? absoluteExpiration = null,
            TimeSpan? absoluteExpirationRelatedToNow = null,
            CancellationToken cancellationToken = default)
        {
            var options = CreateOptions(slidingExpiration, absoluteExpiration, absoluteExpirationRelatedToNow);

            return _distributedCache.SetStringAsync(key, content, options, cancellationToken);
        }


        internal static DistributedCacheEntryOptions CreateOptions(TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration, TimeSpan? absoluteExpirationRelatedToNow)
        {
            return new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration,
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelatedToNow,
                AbsoluteExpiration = absoluteExpiration,
            };
        }
    }
}
