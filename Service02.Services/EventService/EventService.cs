using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Service02.Models;

namespace Service02.Services.EventService
{
    public class EventService : IEventService
    {
        private IOptions<ConnectOption> _options;

        public EventService(IOptions<ConnectOption> options)
        {
            _options = options;
        }

        public async Task CreateEventAsync(long userId, string ipAddress, DateTime connection)
        {
            var request = new EventDto()
            {
                UserId = userId,
                IpAddress = ipAddress,
                Connection = connection,
            };

            var sql = @"
                INSERT INTO event(user_id, ip_address, connection)
                VALUES(@UserId, @IpAddress::inet, @Connection)
                ON CONFLICT ON CONSTRAINT unique_user_ipaddress 
                DO UPDATE SET
                    connection = EXCLUDED.connection;
            ";

            using var conn = new NpgsqlConnection(_options.Value.ConnectionString);
            await conn.ExecuteAsync(sql, request);
        }
    }
}
