using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZeusCaching.Services
{
    public class CachingAdapterFactory : ICachingAdapterFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CachingAdapterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICachingAdapter Create(CachingAdapterMode mode)
        {
            switch (mode)
            {
                case CachingAdapterMode.AutomaticDiscovery:
                    return new AutoDiscoveryCachingAdapter(_serviceProvider);

                case CachingAdapterMode.DistributedCache:
                    var distributedCache = _serviceProvider.GetRequiredService<IDistributedCache>();
                    return new DistributedCachingAdapter(distributedCache);

                case CachingAdapterMode.MemoryCache:
                    var memoryCache = _serviceProvider.GetRequiredService<IMemoryCache>();
                    return new MemoryCachingAdapter(memoryCache);

                case CachingAdapterMode.Custom:
                    return _serviceProvider.GetRequiredService<ICachingAdapter>();

                default:
                    throw new NotSupportedException($"Caching adapter mode {mode} is not supprted.");
            }
        }
    }
}
