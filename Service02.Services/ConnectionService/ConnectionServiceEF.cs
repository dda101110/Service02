using Microsoft.EntityFrameworkCore;
using Service02.Models;
using Service02.Models.Models;

namespace Service02.Services.ConnectionService
{
    public class ConnectionServiceEF : IConnectionService
    {
        private PostgresContext _ctx;

        public ConnectionServiceEF(PostgresContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ConnectionResponse?> GetLastConnectionAsync(long userId)
        {
            var result = await _ctx.Events
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Connection)
                .Select(x => new ConnectionResponse()
                    {
                        IpAddress = x.IpAddress,
                        Connection = x.Connection,
                    })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<DateTime> GetLastTimeConnectionAsync(long userId, string ipAddress)
        {
            var result = await _ctx.Events
                .Where(x => x.UserId == userId && x.IpAddress==ipAddress)
                .Select(x => x.Connection)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
