using Microsoft.AspNetCore.Http;
using System;

namespace ZeusCaching
{
    /// <summary>
    /// Represents a class for caching profile options.
    /// </summary>
    public class ZeusCachingProfileOptions
    {
        internal ZeusCachingProfileOptions()
            : this(true, CachingAdapterMode.DistributedCache,
                 ZeusCachingExtensions.GetCacheKey,
                 ZeusCachingExtensions.DefaultCachingPredicate,
                 ZeusCachingExtensions.DefaultWrappingResultHandler)
        {
        }

        internal ZeusCachingProfileOptions(bool isEnabled,
            CachingAdapterMode cachingAdapterMode,
            Func<IServiceProvider, HttpContext, string> cacheKeyHandler,
            Func<IServiceProvider, HttpRequest, bool> cachingPredicate,
            Func<IServiceProvider, object, object> wrappingResultHandler)
        {
            CachingAdapterMode = cachingAdapterMode;
            IsEnabled = isEnabled;
            CacheKeyHandler = cacheKeyHandler;
            CachingPredicate = cachingPredicate;
            WrappingResultHandler = wrappingResultHandler;
        }

        /// <summary>
        /// Gets or sets caching adapter mode.
        /// </summary>
        internal CachingAdapterMode CachingAdapterMode { get; private set; }


        /// <summary>
        /// Sets the distributed caching adapter mode to <see cref="CachingAdapterMode.AutomaticDiscovery"/>. In this mode, 
        /// the profile options looks for any implementations for <see cref="Microsoft.Extensions.Caching.Distributed.IDistributedCache"/>
        /// or <see cref="Microsoft.Extensions.Caching.Memory.IMemoryCache"/> service to resolve.
        /// </summary>
        /// <returns></returns>
        public ZeusCachingProfileOptions UseAutoDiscoveryCachingAdapter()
        {
            CachingAdapterMode = CachingAdapterMode.AutomaticDiscovery;
            return this;
        }


        /// <summary>
        /// Sets the distributed caching adapter mode to <see cref="CachingAdapterMode.DistributedCache"/> and uses the
        /// underlying <see cref="Microsoft.Extensions.Caching.Distributed.IDistributedCache"/> service instance for caching.
        /// </summary>
        /// <returns></returns>
        public ZeusCachingProfileOptions UseDistributedCachingAdapter()
        {
            CachingAdapterMode = CachingAdapterMode.DistributedCache;
            return this;
        }


        /// <summary>
        /// Sets the distributed caching adapter mode to <see cref="CachingAdapterMode.MemoryCache"/> and uses the
        /// underlying <see cref="Microsoft.Extensions.Caching.Memory.IMemoryCache"/> service instance for caching.
        /// </summary>
        /// <returns></returns>
        public ZeusCachingProfileOptions UseInMemoryCachingAdapter()
        {
            CachingAdapterMode = CachingAdapterMode.MemoryCache;
            return this;
        }




        /// <summary>
        /// Gets or sets the caching profile status.
        /// </summary>
        internal bool IsEnabled { get; private set; }


        /// <summary>
        /// Enables caching for current profile. Caching is enabled by default for this profile if <see cref="ZeusCachingProfileOptions.Disable()"/> is not called.
        /// </summary>
        /// <returns></returns>
        public ZeusCachingProfileOptions Enable()
        {
            IsEnabled = true;
            return this;
        }


        /// <summary>
        /// Disables caching for current profile.
        /// </summary>
        /// <returns></returns>
        public ZeusCachingProfileOptions Disable()
        {
            IsEnabled = false;
            return this;
        }




        /// <summary>
        /// Gets or sets the caching profile key handler.
        /// </summary>
        internal Func<IServiceProvider, HttpContext, string> CacheKeyHandler { get; private set; }


        /// <summary>
        /// Sets a handler for creating caching key.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public ZeusCachingProfileOptions UseCacheKeyHandler(Func<IServiceProvider, HttpContext, string> handler)
        {
            CacheKeyHandler = handler ?? throw new ArgumentNullException(nameof(handler));
            return this;
        }




        /// <summary>
        /// Gets or sets the caching profile predicate function.
        /// </summary>
        internal Func<IServiceProvider, HttpRequest, bool> CachingPredicate { get; private set; }


        /// <summary>
        /// Sets a predicate to determine if the request should be cached.
        /// </summary>
        /// <param name="predicate">A predicate function that determines if the request should be cached.</param>
        /// <returns></returns>
        public ZeusCachingProfileOptions UseCachingPredicate(Func<IServiceProvider, HttpRequest, bool> predicate)
        {
            CachingPredicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            return this;
        }




        /// <summary>
        /// Gets or sets the caching profile wrapping handler.
        /// </summary>
        internal Func<IServiceProvider, object, object> WrappingResultHandler { get; private set; }


        /// <summary>
        /// Sets a handler that can be used to wrap the response.
        /// </summary>
        /// <param name="handler">A handler function that wraps the response.</param>
        /// <returns></returns>
        public ZeusCachingProfileOptions UseWrappingHandler(Func<IServiceProvider, object, object> handler)
        {
            WrappingResultHandler = handler ?? throw new ArgumentNullException(nameof(handler));
            return this;
        }
    }
}
