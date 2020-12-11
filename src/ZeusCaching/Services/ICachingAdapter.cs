using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZeusCaching.Services
{
    public interface ICachingAdapter
    {
        Task SetContentAsync(
            string key,
            string content,
            TimeSpan? slidingExpiration = null,
            DateTimeOffset? absoluteExpiration = null,
            TimeSpan? absoluteExpirationRelatedToNow = null,
            CancellationToken cancellationToken = default);

        Task<string> GetContentAsync(string key, CancellationToken cancellationToken = default);
    }
}
