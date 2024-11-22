using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Caching.Services.Tests
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
        [TestCase("existingKey", true)]
        [TestCase("nonExistingKey", false)]
        public async Task Get_TestCases(string key, bool keyExists)
        {
            // Arrange
            var expectedValue = "cached value";

            object? cacheEntry = keyExists ? expectedValue : null;

            mockMemoryCache.Setup(m => m.TryGetValue(key, out cacheEntry))
                .Returns(true);

            // Act
            var result = await cacheService.GetAsync(key, CancellationToken.None);

            // Assert
            if (keyExists)
            {
                Assert.That(result, Is.EqualTo(expectedValue));
            }
            else
            {
                Assert.IsNull(result);
            }

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
            await cacheService.SetAsync(key, value, duration, CancellationToken.None);

            // Assert
            cacheEntryMock.VerifySet(e => e.AbsoluteExpirationRelativeToNow = duration, Times.Once);

            mockMemoryCache.Verify(m => m.CreateEntry(key), Times.Once);
        }

        [Test]
        [TestCase("existingKey", true, false, Description = "Key exists, no exception")]
        [TestCase("nonExistingKey", true, true, Description = "Key does not exist, no exception")]
        public async Task RemoveKeyAsync_TestCases(string key, bool shouldRemoveKey, bool expectException)
        {
            // Arrange
            if (shouldRemoveKey)
            {
                mockMemoryCache.Setup(m => m.Remove(key));
            }

            if (expectException)
            {
                mockMemoryCache.Setup(m => m.Remove(key)).Throws(new ArgumentNullException(nameof(key)));
            }

            // Act
            var result = await cacheService.RemoveKeyAsync(key, CancellationToken.None);

            // Assert
            if (expectException)
            {
                mockLogger.Verify(x => x.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

                Assert.IsFalse(result);
            }
            else
            {
                mockMemoryCache.Verify(m => m.Remove(key), Times.Once);
                Assert.IsTrue(result);
            }
        }
    }
}