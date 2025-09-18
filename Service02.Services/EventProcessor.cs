using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Service02.Models;

namespace Service02.Services
{
    public sealed class EventProcessor
    {
        private readonly EventQueue _eventQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private IOptions<ConnectOption> _options;

        public EventProcessor(EventQueue queue, IOptions<ConnectOption> options)
        {
            _options = options;
            _eventQueue = queue;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartProcessing()
        {
            _ = ProcessEvents(_cancellationTokenSource.Token);
        }

        public void StopProcessing()
        {
            _cancellationTokenSource.Cancel();
        }

        private async Task ProcessEvents(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_eventQueue.TryDequeue(out var @event))
                {
                    await HandleEventAsync(@event);
                }
                else
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken); 
                }
            }
        }

        private async Task HandleEventAsync(EventDto @event)
        {
            var sql = @"
                INSERT INTO 
                    event(user_id, ip_address, connection)
                VALUES 
                    (@UserId, @IpAddress::inet, @Connection)
                ON CONFLICT ON CONSTRAINT unique_user_ipaddress 
                DO UPDATE SET
                    connection = EXCLUDED.connection;
            ";

            using var conn = new NpgsqlConnection(_options.Value.ConnectionString);
            await conn.ExecuteAsync(sql, @event);
        }
    }
}
