using Dapper;
using Microsoft.Extensions.Options;
using Service02.Models;
using Service02.Services.EventService;
using Xunit;

namespace Service02.Tests
{
    public class EventServiceDapperTest : IDisposable
    {
        private IOptions<ConnectOption> _options;
        private CleanupTestFixture _cleanupTestFixture;

        public EventServiceDapperTest()
        {
            _options = Options.Create(new ConnectOption()
            {
                ConnectionString = "Host=192.168.56.1;Port=5432;Database=postgres_test;Username=postgres;Password=1;",
            });

            _cleanupTestFixture = new CleanupTestFixture(_options);
        }

        [Fact]
        public async Task ValidCreateEventAsync()
        {
            // Arrange
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            await _cleanupTestFixture
                .SeedEventsAsync(commands);

            // Act
            var service = new EventServiceDapper(_options);

            foreach (var command in commands)
            {
                await service.CreateEventAsync(command.UserId,command.IpAddress,command.Connection);
            }

            var conn = _cleanupTestFixture.GetConnection();
            var result = conn
                .Query<EventDto>(@"SELECT 
                                    user_id AS UserId,
                                    host(ip_address) AS IpAddress,
                                    connection 
                                 FROM 
                                    event 
                                WHERE 
                                    user_id=@UserId 
                                    and host(ip_address)=@IpAddress;
                                ", commands[2])
                .ToList();
            var resultCount = result.Count;
            var @event = result.FirstOrDefault();

            // Assert
            Assert.Equal(1, resultCount);
            Assert.Equal(commands[2].Connection, @event?.Connection);
        }

        public void Dispose()
        {
            _cleanupTestFixture.Dispose();
        }
    }
}
