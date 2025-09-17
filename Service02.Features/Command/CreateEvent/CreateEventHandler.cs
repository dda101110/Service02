using Dapper;
using MediatR;
using Microsoft.Extensions.Options;
using Npgsql;
using Service02.Models;

namespace Service02.Features.Command.CreateEvent
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand>
    {
        private string _connectionString;
        public CreateEventHandler(IOptions<ConnectOption> options)
        {
            _connectionString = options.Value.ConnectionString;
        }
        public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var sql = @"
                INSERT INTO event(user_id, ip_address, connection)
                VALUES(@UserId, @IpAddress::inet, @Connection)
                ON CONFLICT ON CONSTRAINT unique_user_ipaddress 
                DO UPDATE SET
                    connection = EXCLUDED.connection;
            ";

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(sql, request);
        }
    }
}
