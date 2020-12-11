using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZeusCaching.Services
{
    public class MemoryCachingAdapter : ICachingAdapter
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCachingAdapter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }



        public Task<string> GetContentAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            var result = _memoryCache.Get<string>(key);
            return Task.FromResult(result);
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

            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            _memoryCache.Set(key, content, options);
            return Task.CompletedTask;
        }

        internal static MemoryCacheEntryOptions CreateOptions(TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration, TimeSpan? absoluteExpirationRelatedToNow)
        {
            return new MemoryCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration,
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelatedToNow,
                AbsoluteExpiration = absoluteExpiration,
            };
        }
    }
}
