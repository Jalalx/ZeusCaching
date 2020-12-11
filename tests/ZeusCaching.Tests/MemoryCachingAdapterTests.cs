using ZeusCaching.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ZeusCaching.Tests
{
    public class MemoryCachingAdapterTests
    {
        private static MemoryCachingAdapter Create()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return new MemoryCachingAdapter(memoryCache);
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
            var options = MemoryCachingAdapter.CreateOptions(slidingExpiration, null, null);


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
            var options = MemoryCachingAdapter.CreateOptions(null, absoluteExpiration, null);

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
            var options = MemoryCachingAdapter.CreateOptions(null, null, absoluteExpirationRelativeToNow);

            // Assert
            Assert.NotNull(options);
            Assert.Null(options.SlidingExpiration);
            Assert.Null(options.AbsoluteExpiration);
            Assert.Equal(absoluteExpirationRelativeToNow, options.AbsoluteExpirationRelativeToNow);
        }
    }
}
