using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Security.Claims;
using System.Text;

namespace Caching.Tests
{
    [TestFixture]
    internal class OutputCachePolicyTests
    {
        private Mock<HttpContext> httpContextMock;
        private OutputCacheContext cacheContext;
        private Mock<HttpRequest> requestMock;
        private Mock<HttpResponse> responseMock;
        private ClaimsPrincipal user;

        [SetUp]
        public void SetUp()
        {

            httpContextMock = new Mock<HttpContext>();
            requestMock = new Mock<HttpRequest>();
            responseMock = new Mock<HttpResponse>();
            user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "userId") }));

            httpContextMock.Setup(ctx => ctx.Request).Returns(requestMock.Object);
            httpContextMock.Setup(ctx => ctx.Response).Returns(responseMock.Object);
            httpContextMock.Setup(ctx => ctx.User).Returns(user);

            cacheContext = new OutputCacheContext() { HttpContext = httpContextMock.Object };
        }

        [Test]
        public async Task CacheRequestAsync_WithDuration_SetsResponseExpirationTimeSpan()
        {
            // Arrange
            var policy = new OutputCachePolicy(TimeSpan.FromMinutes(5), false);
            // Act
            await policy.CacheRequestAsync(cacheContext, CancellationToken.None);
            // Assert
            Assert.That(cacheContext.ResponseExpirationTimeSpan, Is.EqualTo(TimeSpan.FromMinutes(5)));
        }
        [Test]
        public async Task CacheRequestAsync_UseAuthenticationId_AddsUserIdToCacheKeyPrefix()
        {
            // Arrange
            var policy = new OutputCachePolicy(null, true);
            // Act
            await policy.CacheRequestAsync(cacheContext, CancellationToken.None);
            // Assert
            Assert.IsTrue(cacheContext.CacheVaryByRules.CacheKeyPrefix.Contains("userId"));
        }
        [Test]
        public async Task CacheRequestAsync_NoDuration_DoesNotSetExpirationTime()
        {
            // Arrange
            var policy = new OutputCachePolicy(null, false);
            // Act
            await policy.CacheRequestAsync(cacheContext, CancellationToken.None);
            // Assert
            Assert.IsNull(cacheContext.ResponseExpirationTimeSpan);
        }
        [Test]
        public async Task CacheRequestAsync_PostRequestWithJsonContentType_SetsVaryByValues()
        {
            // Arrange
            var policy = new OutputCachePolicy(null, false, "Name", "Id");
            requestMock.Setup(r => r.Method).Returns("POST");
            requestMock.Setup(r => r.ContentType).Returns("application/json");
            var json = "{\"Name\":\"TestName\",\"Id\":\"123\"}";
            requestMock.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(json)));
            // Act
            await policy.CacheRequestAsync(cacheContext, CancellationToken.None);
            // Assert
            Assert.That(cacheContext.CacheVaryByRules.VaryByValues["Name"], Is.EqualTo("TestName"));
            Assert.That(cacheContext.CacheVaryByRules.VaryByValues["Id"], Is.EqualTo("123"));
        }
        [Test]
        public async Task CacheRequestAsync_RequestWithCookie_DisablesCacheStorage()
        {
            // Arrange
            var policy = new OutputCachePolicy();
            responseMock.Setup(r => r.Headers).Returns(new HeaderDictionary
            {
                { "Set-Cookie", new StringValues("testcookie") }
            });
            // Act
            await policy.ServeResponseAsync(cacheContext, CancellationToken.None);
            // Assert
            Assert.IsFalse(cacheContext.AllowCacheStorage);
        }
        [Test]
        public async Task CacheRequestAsync_Non200Or301Status_DisablesCacheStorage()
        {
            // Arrange
            var policy = new OutputCachePolicy();
            responseMock.Setup(r => r.StatusCode).Returns(StatusCodes.Status404NotFound);
            // Act
            await policy.ServeResponseAsync(cacheContext, CancellationToken.None);
            // Assert
            Assert.IsFalse(cacheContext.AllowCacheStorage);
        }
        [Test]
        public async Task AttemptOutputCaching_NonGetOrHeadorPostorPutMethod_ReturnsFalse()
        {
            // Arrange
            requestMock.Setup(r => r.Method).Returns("PATCH");
            var policy = new OutputCachePolicy();
            // Act
            await policy.CacheRequestAsync(cacheContext, CancellationToken.None);
            // Assert
            Assert.IsFalse(cacheContext.AllowCacheLookup);
            Assert.IsFalse(cacheContext.AllowCacheStorage);
        }
        [Test]
        public async Task CacheRequestAsync_RequestWithPropertyNamesToCacheBy_SetsVaryByValues()
        {
            // Arrange
            var policy = new OutputCachePolicy(propertyNamesToCacheBy: ["Category", "Author"]);
            var json = "{\"Category\":\"Fiction\",\"Author\":\"AuthorName\"}";
            requestMock.Setup(r => r.Method).Returns("POST");
            requestMock.Setup(r => r.ContentType).Returns("application/json");
            requestMock.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(json)));
            // Act
            await policy.CacheRequestAsync(cacheContext, CancellationToken.None);
            // Assert
            Assert.That(cacheContext.CacheVaryByRules.VaryByValues["Category"], Is.EqualTo("Fiction"));
            Assert.That(cacheContext.CacheVaryByRules.VaryByValues["Author"], Is.EqualTo("AuthorName"));
        }
    }
}