using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Shared.Services.Tests
{
    [TestFixture]
    internal class InMemoryCacheServiceTests
    {
        private Mock<IMemoryCache> memoryCacheMock;
        private InMemoryCacheService cacheService;

        [SetUp]
        public void Setup()
        {
            memoryCacheMock = new Mock<IMemoryCache>();
            cacheService = new InMemoryCacheService(memoryCacheMock.Object);
        }

        [Test]
        public void Get_KeyExists_ReturnsCachedValue()
        {
            // Arrange
            var key = "existingKey";
            var expectedValue = "cached value";
            object cacheEntry = expectedValue;
            memoryCacheMock
                .Setup(m => m.TryGetValue(key, out cacheEntry))
                .Returns(true);
            // Act
            var result = cacheService.Get<string>(key);
            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
            memoryCacheMock.Verify(m => m.TryGetValue(key, out cacheEntry), Times.Once);
        }
        [Test]
        public void Get_KeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            var key = "nonExistingKey";
            string value = null;
            object cacheEntry = value;
            memoryCacheMock
                .Setup(m => m.TryGetValue(key, out cacheEntry))
                .Returns(true);
            // Act
            var result = cacheService.Get<string>(key);
            // Assert
            Assert.IsNull(result);
            memoryCacheMock.Verify(m => m.TryGetValue(key, out cacheEntry), Times.Once);
        }
        [Test]
        public void Set_ValidKeyAndValue_SetsValueInCacheWithExpiration()
        {
            // Arrange
            var key = "cacheKey";
            var value = "cache value";
            var duration = TimeSpan.FromMinutes(5);
            var cacheEntryMock = new Mock<ICacheEntry>();
            memoryCacheMock
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntryMock.Object);
            // Act
            cacheService.Set(key, value, duration);
            // Assert
            cacheEntryMock.VerifySet(e => e.AbsoluteExpirationRelativeToNow = duration, Times.Once);
            memoryCacheMock.Verify(m => m.CreateEntry(key), Times.Once);
        }
    }
}