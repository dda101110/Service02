using System.Collections.Concurrent;

namespace Service02.Models
{
    public sealed class EventQueue
    {
        private readonly ConcurrentQueue<EventDto> _queue = new ConcurrentQueue<EventDto>();

        public void Enqueue(EventDto @event)
        {
            _queue.Enqueue(@event);
        }

        public bool TryDequeue(out EventDto @event)
        {
            return _queue.TryDequeue(out @event);
        }
    }
}
