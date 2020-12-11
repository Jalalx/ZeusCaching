namespace ZeusCaching
{
    /// <summary>
    /// Represents the caching adapter mode.
    /// </summary>
    public enum CachingAdapterMode
    {
        /// <summary>
        /// Searches for <see cref="Microsoft.Extensions.Caching.Distributed.IDistributedCache"/> or 
        /// <see cref="Microsoft.Extensions.Caching.Memory.IMemoryCache"/> interface implementations in the
        /// current <see cref="System.IServiceProvider"/> instance.
        /// </summary>
        AutomaticDiscovery = 0,

        /// <summary>
        /// Use <see cref="Microsoft.Extensions.Caching.Distributed.IDistributedCache"/> interface.
        /// </summary>
        DistributedCache = 1,

        /// <summary>
        /// Use <see cref="Microsoft.Extensions.Caching.Memory.IMemoryCache"/> interface.
        /// </summary>
        MemoryCache = 2,
    }
}
