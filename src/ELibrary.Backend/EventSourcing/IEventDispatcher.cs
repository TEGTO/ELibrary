namespace EventSourcing
{
    public interface IEventDispatcher
    {
        public Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken);
    }
}