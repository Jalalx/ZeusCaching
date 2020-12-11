using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using ZeusCachingSample;

namespace ZeusCaching.IntegrationTests
{
    public class WeatherForecastControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public WeatherForecastControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Theory]
        [InlineData("weatherforecast/basic")]
        [InlineData("weatherforecast/action-result")]
        [InlineData("weatherforecast/embedded")]
        [InlineData("weatherforecast/scalar")]
        public async Task Get_WithCacheAttribute_EndpointsReturnSuccessAndCorrectResponse(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            var originalResponse = await client.GetAsync(url);

            // Act
            var cachedResponse = await client.GetAsync(url);

            // Assert
            cachedResponse.EnsureSuccessStatusCode();
            Assert.Equal(originalResponse.StatusCode, cachedResponse.StatusCode);
            Assert.Equal(
                originalResponse.Content.Headers.ContentType,
                cachedResponse.Content.Headers.ContentType);
            Assert.Equal(
                await originalResponse.Content.ReadAsStringAsync(),
                await cachedResponse.Content.ReadAsStringAsync());

            Assert.False(originalResponse.Headers.Contains("X-ZeusCaching"));
            Assert.True(cachedResponse.Headers.Contains("X-ZeusCaching"));
        }

        [Theory]
        [InlineData("weatherforecast/disabled")]
        public async Task Get_WithDisabledCacheProfile_EndpointsReturnSuccessAndNewResponse(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            var firstResponse = await client.GetAsync(url);

            // Act
            var secondResponse = await client.GetAsync(url);

            // Assert
            secondResponse.EnsureSuccessStatusCode();
            Assert.Equal(firstResponse.StatusCode, secondResponse.StatusCode);
            Assert.Equal(
                firstResponse.Content.Headers.ContentType.ToString(),
                secondResponse.Content.Headers.ContentType.ToString());
            Assert.NotEqual(
                await firstResponse.Content.ReadAsStringAsync(),
                await secondResponse.Content.ReadAsStringAsync());

            Assert.False(firstResponse.Headers.Contains("X-ZeusCaching"));
            Assert.False(secondResponse.Headers.Contains("X-ZeusCaching"));
        }

    }
}
