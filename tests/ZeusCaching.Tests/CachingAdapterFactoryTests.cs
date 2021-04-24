using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZeusCaching.Services;

namespace ZeusCaching.Tests
{
    public class CachingAdapterFactoryTests
    {
        [Theory]
        [InlineData(CachingAdapterMode.AutomaticDiscovery, typeof(AutoDiscoveryCachingAdapter))]
        [InlineData(CachingAdapterMode.DistributedCache, typeof(DistributedCachingAdapter))]
        [InlineData(CachingAdapterMode.MemoryCache, typeof(MemoryCachingAdapter))]
        [InlineData(CachingAdapterMode.Custom, typeof(FakeCachingAdapter))]
        public void Create_ForGivenMode_ReturnsExpectedAdapter(CachingAdapterMode mode, Type expectedInstanceType)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(IDistributedCache))).Returns(Mock.Of<IDistributedCache>());
            serviceProvider.Setup(x => x.GetService(typeof(IMemoryCache))).Returns(Mock.Of<IMemoryCache>());
            serviceProvider.Setup(x => x.GetService(typeof(ICachingAdapter))).Returns(new FakeCachingAdapter());

            CachingAdapterFactory factory = new CachingAdapterFactory(serviceProvider.Object);


            var adapter = factory.Create(mode);


            Assert.NotNull(adapter);
            Assert.IsType(expectedInstanceType, adapter);
        }

        private class FakeCachingAdapter : ICachingAdapter
        {
            public Task<string> GetContentAsync(string key, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(string.Empty);
            }

            public Task SetContentAsync(string key, string content, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, TimeSpan? absoluteExpirationRelatedToNow = null, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }


        [Fact]
        public void Create_InAutoDiscoveryModeWhenNoCachingServiceIsRegistered_ThrowsException()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var mode = CachingAdapterMode.AutomaticDiscovery;
            CachingAdapterFactory factory = new CachingAdapterFactory(serviceProvider.Object);


            var ex = Record.Exception(() => factory.Create(mode));


            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("No cacing service is registered.", ex.Message);
        }



        [Fact]
        public void Create_ForANotSupportedMode_ThrowsException()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var mode = (CachingAdapterMode)(-1);
            CachingAdapterFactory factory = new CachingAdapterFactory(serviceProvider.Object);


            var ex = Record.Exception(() => factory.Create(mode));


            Assert.NotNull(ex);
            Assert.IsType<NotSupportedException>(ex);
            Assert.Equal($"Caching adapter mode {mode} is not supprted.", ex.Message);
        }
    }
}
