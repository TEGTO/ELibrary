using Moq;

namespace EventSourcing.Tests
{
    [TestFixture]
    public class EventDispatcherTests
    {
        private Mock<IServiceProvider> serviceProviderMock;
        private Mock<IEventHandler<TestEvent>> eventHandlerMock;
        private EventDispatcher eventDispatcher;

        [SetUp]
        public void SetUp()
        {
            serviceProviderMock = new Mock<IServiceProvider>();
            eventHandlerMock = new Mock<IEventHandler<TestEvent>>();

            eventDispatcher = new EventDispatcher(serviceProviderMock.Object);
        }

        [Test]
        public async Task DispatchAsync_WhenHandlerExists_CallsHandleAsync()
        {
            // Arrange
            var testEvent = new TestEvent();

            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IEventHandler<TestEvent>)))
                .Returns(eventHandlerMock.Object);

            // Act
            await eventDispatcher.DispatchAsync(testEvent, CancellationToken.None);

            // Assert
            eventHandlerMock.Verify(handler => handler.HandleAsync(testEvent, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task DispatchAsync_WhenNoHandlerExists_DoesNotCallHandleAsync()
        {
            // Arrange
            var testEvent = new TestEvent();

            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IEventHandler<TestEvent>)))
                .Returns(null!);

            // Act
            await eventDispatcher.DispatchAsync(testEvent, CancellationToken.None);

            // Assert
            eventHandlerMock.Verify(handler => handler.HandleAsync(It.IsAny<TestEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    public class TestEvent { }
}