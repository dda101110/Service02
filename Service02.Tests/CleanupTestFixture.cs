using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Service02.Features.Command.CreateEvent;
using Service02.Models;

namespace Service02.Tests
{
    public class CleanupTestFixture : IDisposable
    {
        private NpgsqlConnection _connection;

        public CleanupTestFixture(IOptions<ConnectOption> options)
        {
            _connection = new NpgsqlConnection(options.Value.ConnectionString);
            _connection.Execute("TRUNCATE TABLE event;");
        }

        public NpgsqlConnection GetConnection()
        {
            return _connection;
        }

        public IEnumerable<CreateEventCommand> GetCommandPack01()
        {
            CreateEventCommand command1 = new CreateEventCommand()
            {
                UserId = 112233,
                IpAddress = "127.0.0.1",
                Connection = new DateTime(2030, 1, 1, 0, 0, 10, kind: DateTimeKind.Utc),
            };
            CreateEventCommand command2 = new CreateEventCommand()
            {
                UserId = 112234,
                IpAddress = "127.0.0.1",
                Connection = new DateTime(2030, 1, 1, 0, 0, 20, kind: DateTimeKind.Utc),
            };
            CreateEventCommand command3 = new CreateEventCommand()
            {
                UserId = 112233,
                IpAddress = "127.0.0.1",
                Connection = new DateTime(2030, 1, 1, 0, 0, 30, kind: DateTimeKind.Utc),
            };
            var commands = new List<CreateEventCommand>() {
                command1,
                command2,
                command3
            };

            return commands;
        }

        public async Task<IEnumerable<CreateEventCommand>> SeedEventsAsync(IEnumerable<CreateEventCommand> commands)
        {
            await _connection
                .ExecuteAsync(@"INSERT INTO 
                                    event (user_id, ip_address, connection)
                                VALUES 
                                    (@UserId, @IpAddress::inet, @Connection)
                                ON CONFLICT ON CONSTRAINT unique_user_ipaddress 
                                DO UPDATE SET
                                    connection = EXCLUDED.connection;", param: commands);

            return commands;
        }

        public void Dispose()
        {
            _connection.Execute("TRUNCATE TABLE event;");
            _connection.Dispose();
        }
    }
}
