using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZeusCaching.Services
{
    public class AutoDiscoveryCachingAdapter : ICachingAdapter
    {
        private readonly ICachingAdapter _adapter;

        public AutoDiscoveryCachingAdapter(IServiceProvider serviceProvider)
        {
            var distributedCache = serviceProvider.GetService(typeof(IDistributedCache)) as IDistributedCache;
            if (distributedCache != null)
            {
                _adapter = new DistributedCachingAdapter(distributedCache);
            }
            else
            {
                var inMemoeryCache = serviceProvider.GetService(typeof(IMemoryCache)) as IMemoryCache;
                if (inMemoeryCache != null)
                {
                    _adapter = new MemoryCachingAdapter(inMemoeryCache);
                }
                else
                {
                    throw new InvalidOperationException("No cacing service is registered.");
                }
            }
        }

        public Task<string> GetContentAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            return _adapter.GetContentAsync(key, cancellationToken);
        }

        public Task SetContentAsync(
            string key,
            string content,
            TimeSpan? slidingExpiration = null,
            DateTimeOffset? absoluteExpiration = null,
            TimeSpan? absoluteExpirationRelatedToNow = null,
            CancellationToken cancellationToken = default)
        {
            return _adapter.SetContentAsync(key, content, slidingExpiration, absoluteExpiration, absoluteExpirationRelatedToNow, cancellationToken);
        }
    }
}
