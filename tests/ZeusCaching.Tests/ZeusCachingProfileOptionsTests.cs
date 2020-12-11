using Microsoft.AspNetCore.Http;
using System;
using Xunit;

namespace ZeusCaching.Tests
{
    public class ZeusCachingProfileOptionsTests
    {
        [Fact]
        public void Enable_ForDefaultInstance_ActsAsExpected()
        {
            var options = new ZeusCachingProfileOptions();


            options.Enable();


            Assert.True(options.IsEnabled);
        }


        [Fact]
        public void Disable_ForDefaultInstance_ActsAsExpected()
        {
            var options = new ZeusCachingProfileOptions();


            options.Disable();


            Assert.False(options.IsEnabled);
        }




        [Fact]
        public void UseAutoDiscoveryCachingAdapter_ForDefaultInstance_ActsAsExpected()
        {
            var options = new ZeusCachingProfileOptions();


            options.UseAutoDiscoveryCachingAdapter();


            Assert.Equal(CachingAdapterMode.AutomaticDiscovery, options.CachingAdapterMode);
        }




        [Fact]
        public void UseDistributedCachingAdapter_ForDefaultInstance_ActsAsExpected()
        {
            var options = new ZeusCachingProfileOptions();


            options.UseDistributedCachingAdapter();


            Assert.Equal(CachingAdapterMode.DistributedCache, options.CachingAdapterMode);
        }




        [Fact]
        public void UseInMemoryCachingAdapter_ForDefaultInstance_ActsAsExpected()
        {
            var options = new ZeusCachingProfileOptions();


            options.UseInMemoryCachingAdapter();


            Assert.Equal(CachingAdapterMode.MemoryCache, options.CachingAdapterMode);
        }




        [Fact]
        public void UseCacheKeyHandler_ForDefaultInstance_ActsAsExpected()
        {
            var options = new ZeusCachingProfileOptions();
            Func<IServiceProvider, HttpContext, string> alwaysFooHandler = (__, _) => "/foo";

            options.UseCacheKeyHandler(alwaysFooHandler);


            Assert.Equal(alwaysFooHandler, options.CacheKeyHandler);
        }




        [Fact]
        public void UseCachingPredicate_ForDefaultInstance_ActsAsExpected()
        {
            var options = new ZeusCachingProfileOptions();
            Func<IServiceProvider, HttpRequest, bool> alwaysTrueHandler = (__, _) => true;


            options.UseCachingPredicate(alwaysTrueHandler);


            Assert.Equal(alwaysTrueHandler, options.CachingPredicate);
        }




        [Fact]
        public void UseWrappingHandler_ForDefaultInstance_ActsAsExpected()
        {
            var options = new ZeusCachingProfileOptions();
            Func<IServiceProvider, object, object> wrapInResultHandler = (__, content) => new { Result = content };


            options.UseWrappingHandler(wrapInResultHandler);


            Assert.Equal(wrapInResultHandler, options.WrappingResultHandler);
        }
    }
}
