using System;
using Xunit;

namespace ZeusCaching.Tests
{
    public class ZeusCachingOptionsBuilderTests
    {
        [Fact]
        public void Build_WithNoProfile_ThrowsInvalidOperationException()
        {
            var builder = new ZeusCachingOptionsBuilder();


            var ex = Record.Exception(() => builder.Build());


            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("No profile is defined. Please call AddDefaultProfile or AddNamedProfile before building the options.", ex.Message);
        }




        [Fact]
        public void Build_WithAddDefaultProfileAndNoConfigurations_ReturnsDefaultOptions()
        {
            var builder = new ZeusCachingOptionsBuilder();



            builder.AddDefaultProfile();
            var options = builder.Build();
            var defaultOptions = options.GetOptions(string.Empty);


            Assert.NotNull(defaultOptions);
            Assert.True(defaultOptions.IsEnabled);
            Assert.Equal(ZeusCachingExtensions.GetCacheKey, defaultOptions.CacheKeyHandler);
            Assert.Equal(CachingAdapterMode.DistributedCache, defaultOptions.CachingAdapterMode);
            Assert.Equal(ZeusCachingExtensions.DefaultCachingPredicate, defaultOptions.CachingPredicate);
            Assert.Equal(ZeusCachingExtensions.DefaultWrappingResultHandler, defaultOptions.WrappingResultHandler);
        }




        [Fact]
        public void Build_WithAddNamedProfileAndNoConfigurations_ReturnsDefaultOptions()
        {
            var builder = new ZeusCachingOptionsBuilder();



            builder.AddNamedProfile("MyProfile1");
            var options = builder.Build();
            var defaultOptions = options.GetOptions("MyProfile1");


            Assert.NotNull(defaultOptions);
            Assert.True(defaultOptions.IsEnabled);
            Assert.Equal(ZeusCachingExtensions.GetCacheKey, defaultOptions.CacheKeyHandler);
            Assert.Equal(CachingAdapterMode.DistributedCache, defaultOptions.CachingAdapterMode);
            Assert.Equal(ZeusCachingExtensions.DefaultCachingPredicate, defaultOptions.CachingPredicate);
            Assert.Equal(ZeusCachingExtensions.DefaultWrappingResultHandler, defaultOptions.WrappingResultHandler);
        }
    }
}
