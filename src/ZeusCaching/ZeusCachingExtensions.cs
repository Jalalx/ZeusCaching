using ZeusCaching.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;

namespace ZeusCaching
{
    public static class ZeusCachingExtensions
    {
        internal const string DefaultProfileName = "ZeusCachingDefaultProfile";

        public static IServiceCollection AddZeusCaching(this IServiceCollection services)
        {
            var hasDistributedCache = services.Any(x => x.ServiceType == typeof(IDistributedCache));
            var hasMemoryCache = services.Any(x => x.ServiceType == typeof(IMemoryCache));

            if (!hasDistributedCache && !hasMemoryCache)
            {
                throw new InvalidOperationException("No caching is registered for this application. Please consider registering a IMemoryCache or IDistributedCache service.");
            }

            return AddZeusCaching(services, (b) => b.AddDefaultProfile());
        }


        public static IServiceCollection AddZeusCaching(this IServiceCollection services, Action<ZeusCachingOptionsBuilder> optionsBuilder = null)
        {
            var builder = new ZeusCachingOptionsBuilder();
            if (optionsBuilder != null)
            {
                optionsBuilder(builder);
            }

            var options = builder.Build();
            services.AddSingleton(options);

            services.AddTransient<IActionResultContentAdapter, DefaultActionResultContentAdapter>();
            services.AddTransient<ICachingAdapterFactory, CachingAdapterFactory>();
            services.AddTransient<IZeusCachingProfileResolver, ZeusCachingProfileResolver>();
            services.AddTransient<IZeusCachingService, ZeusCachingService>();

            return services;
        }



        internal static string GetCacheKey(IServiceProvider _, HttpContext context)
        {
            var builder = new StringBuilder();
            builder.Append($":{context.Request.Path}");
            foreach (var (key, value) in context.Request.Query.OrderBy(x => x.Key))
            {
                builder.Append($"{key}={value}");
            }

            return builder.ToString();
        }



        internal static bool DefaultCachingPredicate(IServiceProvider _, HttpRequest __) => true;



        internal static object DefaultWrappingResultHandler(IServiceProvider _, object obj) => obj;
    }
}
