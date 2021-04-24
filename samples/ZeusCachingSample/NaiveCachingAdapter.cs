using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZeusCaching.Services;

namespace ZeusCachingSample
{
    public class NaiveCachingAdapter : ICachingAdapter
    {
        private readonly Dictionary<string, string> _entries = new Dictionary<string, string>();

        public Task<string> GetContentAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_entries.ContainsKey(key))
            {
                return Task.FromResult(_entries[key]);
            }

            return Task.FromResult(default(string));
        }

        public Task SetContentAsync(string key, string content, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, TimeSpan? absoluteExpirationRelatedToNow = null, CancellationToken cancellationToken = default)
        {
            _entries[key] = content;
            return Task.CompletedTask;
        }
    }
}
