namespace Service02.Services.EventService
{
    public interface IEventService
    {
        Task CreateEventAsync(long userId, string ipAddress, DateTime connection);
    }
}
