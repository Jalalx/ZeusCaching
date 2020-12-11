using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ZeusCaching.Services
{
    public class ZeusCachingContext
    {
        public ZeusCachingContext(
            ActionExecutingContext context,
            string profileName,
            DateTimeOffset? absoluteExpiration,
            TimeSpan? absoluteExpirationRelativeToNow,
            TimeSpan? slidingExpiration)
        {
            ExecutionContext = context;
            ProfileName = profileName;
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            AbsoluteExpiration = absoluteExpiration;
            SlidingExpiration = slidingExpiration;
        }

        public ActionExecutingContext ExecutionContext { get; set; }

        public string ProfileName { get; set; }

        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

        public DateTimeOffset? AbsoluteExpiration { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }
    }
}
