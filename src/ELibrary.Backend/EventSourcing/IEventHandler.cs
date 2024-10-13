namespace EventSourcing
{
    public interface IEventHandler<TEvent>
    {
        public Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}
