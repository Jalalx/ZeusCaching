using ZeusCaching.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ZeusCaching
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ZeusCacheAttribute : Attribute, IAsyncActionFilter, IOrderedFilter
    {
        public ZeusCacheAttribute(int absoluteExpirationInSeconds) : this("", absoluteExpirationInSeconds, 0)
        {
        }

        public ZeusCacheAttribute(string profileName, int absoluteExpirationInSeconds) : this(profileName, absoluteExpirationInSeconds, 0)
        {
        }

        public ZeusCacheAttribute(int absoluteExpirationInSeconds = 0, int slidingExpirationInSeconds = 0) : this("", absoluteExpirationInSeconds, slidingExpirationInSeconds)
        {
        }

        public ZeusCacheAttribute(string profileName = "", int absoluteExpirationInSeconds = 0, int slidingExpirationInSeconds = 0)
        {
            if (absoluteExpirationInSeconds != 0)
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(absoluteExpirationInSeconds);
            }

            if (slidingExpirationInSeconds != 0)
            {
                SlidingExpiration = TimeSpan.FromSeconds(slidingExpirationInSeconds);
            }

            ProfileName = profileName;
        }

        public TimeSpan? AbsoluteExpirationRelativeToNow { get; }

        public TimeSpan? SlidingExpiration { get; }

        public string ProfileName { get; } = string.Empty;

        public int Order { get; } = int.MaxValue;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var service = serviceProvider.GetRequiredService<IZeusCachingService>();

            var cachingContext = new ZeusCachingContext(context, ProfileName, null, AbsoluteExpirationRelativeToNow, SlidingExpiration);
            await service.ProcessRequestAsync(cachingContext, next);
        }
    }
}
