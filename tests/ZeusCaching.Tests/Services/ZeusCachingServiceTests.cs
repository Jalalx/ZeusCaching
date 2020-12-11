using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using ZeusCaching;
using ZeusCaching.Services;

namespace ZeusCaching.Tests.Services
{
    public class ZeusCachingServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IServiceProvider> mockServiceProvider;
        private Mock<ICachingAdapterFactory> mockCachingAdapterFactory;
        private Mock<IZeusCachingProfileResolver> mockZeusCachingProfileResolver;
        private Mock<IActionResultContentAdapter> mockActionResultContentAdapter;
        private Mock<ZeusCachingOptions> mockZeusCachingOptions;

        public ZeusCachingServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>();
            this.mockCachingAdapterFactory = this.mockRepository.Create<ICachingAdapterFactory>();
            this.mockZeusCachingProfileResolver = this.mockRepository.Create<IZeusCachingProfileResolver>();
            this.mockActionResultContentAdapter = this.mockRepository.Create<IActionResultContentAdapter>();
            this.mockZeusCachingOptions = this.mockRepository.Create<ZeusCachingOptions>();
        }

        private ZeusCachingService CreateService()
        {
            return new ZeusCachingService(
                this.mockServiceProvider.Object,
                this.mockCachingAdapterFactory.Object,
                this.mockZeusCachingProfileResolver.Object,
                this.mockActionResultContentAdapter.Object,
                this.mockZeusCachingOptions.Object);
        }

        //[Fact]
        //public async Task ProcessRequestAsync_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var service = this.CreateService();
        //    ZeusCachingContext context = null;
        //    ActionExecutionDelegate next = null;

        //    // Act
        //    await service.ProcessRequestAsync(
        //        context,
        //        next);

        //    // Assert
        //    Assert.True(false);
        //    this.mockRepository.VerifyAll();
        //}
    }
}
