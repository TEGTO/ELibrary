using Caching.Services;
using Moq;
using System.Text.Json;

namespace Caching.Tests
{
    [TestFixture]
    internal class CacheExtensionsTests
    {
        private Mock<ICacheService> cacheServiceMock;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            cacheServiceMock = new Mock<ICacheService>();
            cancellationToken = CancellationToken.None;
        }

        [Test]
        public void TryGetAsync_CacheServiceIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICacheService nullCacheService = null;
            //Act + Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await nullCacheService.TryGetAsync<string>("key", cancellationToken));
        }
        [Test]
        public async Task TryGetAsync_ValidJson_ReturnsDeserializedObject()
        {
            //Arrange
            var key = "testKey";
            var expectedValue = new TestObject { Id = 1, Name = "Test" };
            var serializedValue = JsonSerializer.Serialize(expectedValue);
            cacheServiceMock.Setup(s => s.GetAsync(key, cancellationToken)).ReturnsAsync(serializedValue);
            //Act
            var result = await cacheServiceMock.Object.TryGetAsync<TestObject>(key, cancellationToken);
            //Assert
            Assert.IsNotNull(result);
            Assert.That(result?.Id, Is.EqualTo(expectedValue.Id));
            Assert.That(result?.Name, Is.EqualTo(expectedValue.Name));
        }
        [Test]
        public async Task TryGetAsync_InvalidJson_ReturnsDefault()
        {
            //Arrange
            var key = "testKey";
            cacheServiceMock.Setup(s => s.GetAsync(key, cancellationToken)).ReturnsAsync("invalid json");
            //Act
            var result = await cacheServiceMock.Object.TryGetAsync<TestObject>(key, cancellationToken);
            //Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task TryGetAsync_EmptyCache_ReturnsDefault()
        {
            //Arrange
            var key = "testKey";
            cacheServiceMock.Setup(s => s.GetAsync(key, cancellationToken)).ReturnsAsync((string)null);
            //Act
            var result = await cacheServiceMock.Object.TryGetAsync<TestObject>(key, cancellationToken);
            //Assert
            Assert.IsNull(result);
        }
        [Test]
        public void TrySetAsync_CacheServiceIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICacheService nullCacheService = null;
            var testObject = new TestObject { Id = 1, Name = "Test" };
            //Act + Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await nullCacheService.TrySetAsync("key", testObject, TimeSpan.FromMinutes(5), cancellationToken));
        }
        [Test]
        public async Task TrySetAsync_ValidObject_ReturnsTrue()
        {
            //Arrange
            var key = "testKey";
            var value = new TestObject { Id = 1, Name = "Test" };
            var serializedValue = JsonSerializer.Serialize(value);
            cacheServiceMock.Setup(s => s.SetAsync(key, serializedValue, It.IsAny<TimeSpan>(), cancellationToken)).ReturnsAsync(true);
            //Act
            var result = await cacheServiceMock.Object.TrySetAsync(key, value, TimeSpan.FromMinutes(5), cancellationToken);
            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task TrySetAsync_SerializationFails_ReturnsFalse()
        {
            //Arrange
            var key = "testKey";
            var value = new TestObject { Id = 1, Name = "Test" };
            cacheServiceMock.Setup(s => s.SetAsync(key, It.IsAny<string>(), It.IsAny<TimeSpan>(), cancellationToken)).Throws<JsonException>();
            //Act
            var result = await cacheServiceMock.Object.TrySetAsync(key, value, TimeSpan.FromMinutes(5), cancellationToken);
            //Assert
            Assert.IsFalse(result);
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}