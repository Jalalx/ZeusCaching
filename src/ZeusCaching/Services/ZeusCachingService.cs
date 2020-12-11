using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZeusCaching.Services
{
    public class ZeusCachingService : IZeusCachingService
    {
        const string GlobalDisableVariableName = "ZEUS_CACHING_DISABLED";
        const string ZeusCachingHeaderName = "X-ZeusCaching";
        private static Lazy<bool> IsDisabledGlobally = new Lazy<bool>(InternalIsDisabledGlobally);

        private static bool InternalIsDisabledGlobally()
        {
            return bool.TryParse(Environment.GetEnvironmentVariable(GlobalDisableVariableName), out bool isDisabled) ? isDisabled : false;
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly ICachingAdapterFactory _cachingAdapterFactory;
        private readonly IZeusCachingProfileResolver _ZeusCachingProfileResolver;
        private readonly IActionResultContentAdapter _actionResultCachingContextAdapter;
        private readonly ZeusCachingOptions _options;

        public ZeusCachingService(
            IServiceProvider serviceProvider,
            ICachingAdapterFactory cachingAdapterFactory,
            IZeusCachingProfileResolver ZeusCachingProfileResolver,
            IActionResultContentAdapter actionResultCachingContextAdapter,
            ZeusCachingOptions options)
        {
            _serviceProvider = serviceProvider;
            _cachingAdapterFactory = cachingAdapterFactory;
            _ZeusCachingProfileResolver = ZeusCachingProfileResolver;
            _actionResultCachingContextAdapter = actionResultCachingContextAdapter;
            _options = options;
        }

        public async Task ProcessRequestAsync(ZeusCachingContext context, ActionExecutionDelegate next)
        {
            if (!_options.IsEnabled || IsDisabledGlobally.Value)
            {
                await next();
                return;
            }

            var profileOptions = _ZeusCachingProfileResolver.GetOptions(context.ProfileName);
            if (!profileOptions.IsEnabled)
            {
                await next();
                return;
            }

            if (!profileOptions.CachingPredicate(_serviceProvider, context.ExecutionContext.HttpContext.Request))
            {
                await next();
                return;
            }

            if (profileOptions.CacheKeyHandler == null)
            {
                throw new InvalidOperationException("No CacheKeyHandler is registered.");
            }
            
            var httpResponse = context.ExecutionContext.HttpContext.Response;
            var key = profileOptions.CacheKeyHandler(_serviceProvider, context.ExecutionContext.HttpContext);
            var cachingAdapter = _cachingAdapterFactory.Create(profileOptions.CachingAdapterMode);
            var response = await cachingAdapter.GetContentAsync(key);

            if (string.IsNullOrWhiteSpace(response))
            {
                var resultContext = await next?.Invoke();

                // Don't cache errors
                if (resultContext.Exception != null)
                {
                    return;
                }

                if (IsSuccessStatusCode(httpResponse.StatusCode))
                {
                    if (_actionResultCachingContextAdapter.TryGetContent(resultContext.Result, out object content))
                    {
                        await SetContentToCacheAsync(context, profileOptions, key, content, cachingAdapter);
                    }
                    else
                    {
                        // Only supported ActionResult content gets cached.
                    }
                }
            }
            else
            {
                context.ExecutionContext.Result = _actionResultCachingContextAdapter.CreateResult(response);
                httpResponse.Headers[ZeusCachingHeaderName] = $"{response?.Length}";
            }
        }


        private static bool IsSuccessStatusCode(int statusCode) => statusCode / 100 == 2;


        protected virtual async Task SetContentToCacheAsync(
            ZeusCachingContext context,
            ZeusCachingProfileOptions options,
            string key,
            object content,
            ICachingAdapter cachingAdapter)
        {
            object wrappedContent;
            if (options.WrappingResultHandler != null)
            {
                wrappedContent = options.WrappingResultHandler(_serviceProvider, content);
            }
            else
            {
                wrappedContent = content;
            }

            if (wrappedContent is string)
            {
                await cachingAdapter.SetContentAsync(key, (string)wrappedContent, context.SlidingExpiration, null, context.AbsoluteExpirationRelativeToNow);
            }
            else
            {
                // Serialize the response
                var json = Serializer(wrappedContent);
                await cachingAdapter.SetContentAsync(key, json, context.SlidingExpiration, null, context.AbsoluteExpirationRelativeToNow);
            }
        }


        protected virtual string Serializer(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            return JsonSerializer.Serialize(obj, options);
        }
    }
}
