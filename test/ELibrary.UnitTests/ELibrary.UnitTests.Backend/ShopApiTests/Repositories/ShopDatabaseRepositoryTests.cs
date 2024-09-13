using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MockQueryable.Moq;
using Moq;

namespace ShopApi.Repositories.Tests
{
    [TestFixture]
    internal class ShopDatabaseRepositoryTests
    {
        private Mock<IDbContextFactory<LibraryShopDbContext>> dbContextFactoryMock;
        private Mock<LibraryShopDbContext> mockDbContext;
        private Mock<DatabaseFacade> mockDatabase;
        private ShopDatabaseRepository shopDatabaseRepository;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            mockDbContext = new Mock<LibraryShopDbContext>(new DbContextOptionsBuilder<LibraryShopDbContext>()
                .UseSqlite("Filename=:memory:")
                .Options);

            mockDatabase = new Mock<DatabaseFacade>(mockDbContext.Object);

            mockDbContext.Setup(x => x.Database).Returns(mockDatabase.Object);

            dbContextFactoryMock = new Mock<IDbContextFactory<LibraryShopDbContext>>();
            dbContextFactoryMock.Setup(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                                .ReturnsAsync(mockDbContext.Object);

            shopDatabaseRepository = new ShopDatabaseRepository(dbContextFactoryMock.Object);
            cancellationToken = new CancellationToken();
        }
        private static Mock<DbSet<T>> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }

        [Test]
        public async Task CreateOrderAsync_ValidOrder_OrderCreated()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Test Address", Books = new List<Book>() };
            var bookIds = new List<int> { 1, 2 };
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Book 1" },
                new Book { Id = 2, Name = "Book 2" }
            };
            var mockBookQueryable = GetDbSetMock(books);
            mockDbContext.Setup(db => db.Set<Book>()).Returns(mockBookQueryable.Object);
            mockDbContext.Setup(db => db.AddAsync(order, It.IsAny<CancellationToken>()));
            // Act
            var result = await shopDatabaseRepository.CreateOrderAsync(order, bookIds, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Books.Count, Is.EqualTo(2));
            mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockDbContext.Verify(db => db.AddAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task UpdateOrderAsync_ValidOrder_OrderUpdated()
        {
            // Arrange
            var existingOrder = new Order { Id = 1, DeliveryAddress = "Old Address", Books = new List<Book>() };
            var updatedOrder = new Order { Id = 1, DeliveryAddress = "Updated Address", Books = new List<Book>() };
            var bookIds = new List<int> { 1 };
            var books = new List<Book> { new Book { Id = 1, Name = "Book 1" } };
            var mockBookQueryable = GetDbSetMock(books);
            var mockOrderQueryable = GetDbSetMock(new List<Order> { existingOrder });
            mockDbContext.Setup(db => db.Set<Book>()).Returns(mockBookQueryable.Object);
            mockDbContext.Setup(db => db.Orders).Returns(mockOrderQueryable.Object);
            // Act
            var result = await shopDatabaseRepository.UpdateOrderAsync(updatedOrder, bookIds, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.DeliveryAddress, Is.EqualTo("Updated Address"));
            Assert.That(result.Books.Count, Is.EqualTo(1));
            mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void UpdateOrderAsync_OrderNotFound_ThrowsException()
        {
            // Arrange
            var updatedOrder = new Order { Id = 999, DeliveryAddress = "Updated Address" };
            var bookIds = new List<int> { 1 };
            var mockOrderQueryable = GetDbSetMock(new List<Order>());
            mockDbContext.Setup(db => db.Orders).Returns(mockOrderQueryable.Object);
            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
                await shopDatabaseRepository.UpdateOrderAsync(updatedOrder, bookIds, cancellationToken), "Order not found.");
        }
    }
}