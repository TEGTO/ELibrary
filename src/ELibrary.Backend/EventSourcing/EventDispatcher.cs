namespace EventSourcing
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public EventDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetService(typeof(IEventHandler<TEvent>)) as IEventHandler<TEvent>;
            if (handler != null)
            {
                await handler.HandleAsync(@event, cancellationToken);
            }
        }
    }
}
