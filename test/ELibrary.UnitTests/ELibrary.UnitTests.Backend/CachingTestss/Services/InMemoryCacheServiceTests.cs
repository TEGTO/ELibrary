using Caching.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Shared.Services.Tests
{
    [TestFixture]
    internal class InMemoryCacheServiceTests
    {
        private Mock<IMemoryCache> mockMemoryCache;
        private Mock<ILogger<InMemoryCacheService>> mockLogger;
        private InMemoryCacheService cacheService;

        [SetUp]
        public void Setup()
        {
            mockMemoryCache = new Mock<IMemoryCache>();
            mockLogger = new Mock<ILogger<InMemoryCacheService>>();
            cacheService = new InMemoryCacheService(mockMemoryCache.Object, mockLogger.Object);
        }

        [Test]
        public async Task Get_KeyExists_ReturnsCachedValue()
        {
            // Arrange
            var key = "existingKey";
            var expectedValue = "cached value";
            object cacheEntry = expectedValue;
            mockMemoryCache
                .Setup(m => m.TryGetValue(key, out cacheEntry))
                .Returns(true);
            // Act
            var result = await cacheService.GetAsync<string>(key);
            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
            mockMemoryCache.Verify(m => m.TryGetValue(key, out cacheEntry), Times.Once);
        }
        [Test]
        public async Task Get_KeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            var key = "nonExistingKey";
            string value = null;
            object cacheEntry = value;
            mockMemoryCache
                .Setup(m => m.TryGetValue(key, out cacheEntry))
                .Returns(true);
            // Act
            var result = await cacheService.GetAsync<string>(key);
            // Assert
            Assert.IsNull(result);
            mockMemoryCache.Verify(m => m.TryGetValue(key, out cacheEntry), Times.Once);
        }
        [Test]
        public async Task Set_ValidKeyAndValue_SetsValueInCacheWithExpiration()
        {
            // Arrange
            var key = "cacheKey";
            var value = "cache value";
            var duration = TimeSpan.FromMinutes(5);
            var cacheEntryMock = new Mock<ICacheEntry>();
            mockMemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntryMock.Object);
            // Act
            await cacheService.SetAsync(key, value, duration);
            // Assert
            cacheEntryMock.VerifySet(e => e.AbsoluteExpirationRelativeToNow = duration, Times.Once);
            mockMemoryCache.Verify(m => m.CreateEntry(key), Times.Once);
        }
    }
}