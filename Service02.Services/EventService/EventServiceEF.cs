using Service02.Models.Models;

namespace Service02.Services.EventService
{
    public class EventServiceEF : IEventService
    {
        private PostgresContext _ctx;

        public EventServiceEF(PostgresContext ctx)
        {
            _ctx = ctx;
        }

        public async Task CreateEventAsync(long userId, string ipAddress, DateTime connection)
        {
            var @event = _ctx.Events
                .Where(x=>x.UserId== userId && x.IpAddress==ipAddress)
                .FirstOrDefault();

            if (@event != null)
            {
                @event.Connection = connection;
            } else
            {
                @event = new Event()
                {
                    UserId = userId,
                    IpAddress = ipAddress,
                    Connection = connection,
                };

                _ctx.Events.Add(@event);
            }

            await _ctx.SaveChangesAsync();
        }
    }
}
