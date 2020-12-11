using ZeusCaching.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using Xunit;

namespace ZeusCaching.Tests
{
    public class CachingAdapterFactoryTests
    {
        [Theory]
        [InlineData(CachingAdapterMode.AutomaticDiscovery, typeof(AutoDiscoveryCachingAdapter))]
        [InlineData(CachingAdapterMode.DistributedCache, typeof(DistributedCachingAdapter))]
        [InlineData(CachingAdapterMode.MemoryCache, typeof(MemoryCachingAdapter))]
        public void Create_ForGivenMode_ReturnsExpectedAdapter(CachingAdapterMode mode, Type expectedInstanceType)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(IDistributedCache))).Returns(Mock.Of<IDistributedCache>());
            serviceProvider.Setup(x => x.GetService(typeof(IMemoryCache))).Returns(Mock.Of<IMemoryCache>());

            CachingAdapterFactory factory = new CachingAdapterFactory(serviceProvider.Object);


            var adapter = factory.Create(mode);


            Assert.NotNull(adapter);
            Assert.IsType(expectedInstanceType, adapter);
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
