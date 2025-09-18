using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Service02.Models;

namespace Service02.Services.ConnectionService
{
    public class ConnectionServiceDapper : IConnectionService
    {
        private IOptions<ConnectOption> _options;

        public ConnectionServiceDapper(IOptions<ConnectOption> options)
        {
            _options = options;
        }

        public async Task<ConnectionResponse> GetLastConnectionAsync(long userId)
        {
            var sql = @$"
                SELECT 
                    host(ip_address) as IpAddress, connection 
                FROM 
                    event 
                WHERE 
                    user_id={userId}
                ORDER BY connection DESC
                LIMIT 1;
            ";

            using var conn = new NpgsqlConnection(_options.Value.ConnectionString);
            var result = (await conn.QueryAsync<ConnectionResponse>(sql)).FirstOrDefault();

            return result;
        }

        public async Task<DateTime> GetLastTimeConnectionAsync(long userId, string ipAddress)
        {
            var sql = @$"
                SELECT 
                    connection
                FROM 
                    event 
                WHERE 
                    user_id={userId}
                    and ip_address='{ipAddress}'::inet;
            ";

            using var conn = new NpgsqlConnection(_options.Value.ConnectionString);
            var result = (await conn.QueryAsync<DateTime>(sql)).FirstOrDefault();

            return result;
        }
    }
}
