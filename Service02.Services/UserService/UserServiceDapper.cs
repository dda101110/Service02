using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Service02.Models;

namespace Service02.Services.UserService
{
    public class UserServiceDapper : IUserService
    {
        private IOptions<ConnectOption> _options;

        public UserServiceDapper(IOptions<ConnectOption> options)
        {
            _options = options;
        }

        public async Task<IEnumerable<long>> GetUsersByIpAddressAsync(string ipAddress)
        {
            var sql = @$"
                SELECT 
                    DISTINCT user_id 
                FROM 
                    event 
                WHERE 
                    host(ip_address) LIKE '{ipAddress}%';
            ";

            using var conn = new NpgsqlConnection(_options.Value.ConnectionString);
            var result = await conn.QueryAsync<long>(sql);

            return result;
        }
    }
}
