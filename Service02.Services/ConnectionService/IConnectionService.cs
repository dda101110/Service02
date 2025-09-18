using Service02.Models;

namespace Service02.Services.ConnectionService
{
    public interface IConnectionService
    {
        Task<ConnectionResponse?> GetLastConnectionAsync(long userId);
        Task<DateTime> GetLastTimeConnectionAsync(long userId, string ipAddress);
    }
}
