using ZeusCaching.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ZeusCaching.Tests
{
    public class DistributedCachingAdapterTests
    {
        private static DistributedCachingAdapter Create()
        {
            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var distributedCache = serviceProvider.GetService<IDistributedCache>();
            return new DistributedCachingAdapter(distributedCache);
        }



        [Fact]
        public async Task GetContent_ForValidKeyAndExistingContent_ReturnsExpectedValue()
        {
            // Arrange
            var key = "key1";
            var content = "content 1";
            var adapter = Create();
            var absoluteExpiration = TimeSpan.FromMinutes(1);

            await adapter.SetContentAsync(key, content, absoluteExpiration);

            // Act
            var actualContent = await adapter.GetContentAsync(key);


            // Assert
            Assert.NotNull(actualContent);
            Assert.Equal(content, actualContent);
        }



        [Fact]
        public async Task GetContent_ForNewKeyAndNoContent_ReturnsExpectedValue()
        {
            // Arrange
            var key = "key1";
            var adapter = Create();

            // Act
            var actualContent = await adapter.GetContentAsync(key);


            // Assert
            Assert.Null(actualContent);
        }



        [Fact]
        public void CreateOptions_WithSlidingExpiration_ReturnsExpectedOptions()
        {
            // Arrange
            var slidingExpiration = TimeSpan.FromMinutes(2);

            // Act
            var options = DistributedCachingAdapter.CreateOptions(slidingExpiration, null, null);


            Assert.NotNull(options);
            Assert.Equal(TimeSpan.FromMinutes(2), options.SlidingExpiration);
            Assert.Null(options.AbsoluteExpiration);
            Assert.Null(options.AbsoluteExpirationRelativeToNow);
        }



        [Fact]
        public void CreateOptions_WithAbsoluteExpiration_ReturnsExpectedOptions()
        {
            // Arrange
            var absoluteExpiration = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero) + TimeSpan.FromMinutes(2);

            // Act
            var options = DistributedCachingAdapter.CreateOptions(null, absoluteExpiration, null);

            // Assert
            Assert.NotNull(options);
            Assert.Null(options.SlidingExpiration);
            Assert.Equal(absoluteExpiration, options.AbsoluteExpiration);
            Assert.Null(options.AbsoluteExpirationRelativeToNow);
        }



        [Fact]
        public void CreateOptions_WithAbsoluteExpirationRelativeToNow_ReturnsExpectedOptions()
        {
            // Arrange
            var absoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);

            // Act
            var options = DistributedCachingAdapter.CreateOptions(null, null, absoluteExpirationRelativeToNow);

            // Assert
            Assert.NotNull(options);
            Assert.Null(options.SlidingExpiration);
            Assert.Null(options.AbsoluteExpiration);
            Assert.Equal(absoluteExpirationRelativeToNow, options.AbsoluteExpirationRelativeToNow);
        }
    }
}
