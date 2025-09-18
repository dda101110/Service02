using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Service02.Models;

namespace Service02.Services.IpService
{
    public class IpServiceDapper : IIpService
    {
        private IOptions<ConnectOption> _options;

        public IpServiceDapper(IOptions<ConnectOption> options)
        {
            _options = options;
        }

        public async Task<IEnumerable<string>> GetIpAddressesAsync(long userId)
        {
            var sql = @$"
                SELECT 
                    host(ip_address) 
                FROM 
                    event 
                WHERE 
                    user_id={userId};
            ";

            using var conn = new NpgsqlConnection(_options.Value.ConnectionString);
            var result = await conn.QueryAsync<string>(sql);

            return result;
        }
    }
}
